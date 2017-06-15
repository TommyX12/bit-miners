using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Grid<TData> where TData : class
{
	
	public delegate IGridElement<TData> ConstructElementCallback(Grid<TData> grid);

	public delegate void ApplySamplerDataFunction(Grid<TData> grid, GridCoord coord, float pixel);
	
	#region Members
	
	public Dictionary<GridCoord, TData> m_data;
	private Dictionary<GridCoord, IGridElement<TData>> m_elements;
	private Stack<IGridElement<TData>> m_inactiveElements;
	private HashSet<GridCoord> m_activeBatches;
	
	#endregion


	#region Constants

	#endregion


	#region Properties

	public float TileSize {get; private set;}
	
	public bool AutoElementManagement {get; private set;}
	public int BatchRadius {get; private set;}
	public Transform ParentTransform {get; private set;}
	
	public ConstructElementCallback ConstructElementFunc {get; private set;}

	#endregion


	#region Constructors

	public Grid(float tileSize, bool autoElementManagement, int batchRadius, Transform parentTransform, ConstructElementCallback constructElementFunc) {
		this.TileSize = tileSize;
		
		this.AutoElementManagement = autoElementManagement;
		this.BatchRadius = batchRadius;
		this.ParentTransform = parentTransform;
		
		this.ConstructElementFunc = constructElementFunc;
		
		this.m_data = new Dictionary<GridCoord, TData>();
		this.m_elements = new Dictionary<GridCoord, IGridElement<TData>>();
		this.m_inactiveElements = new Stack<IGridElement<TData>>();
		this.m_activeBatches = new HashSet<GridCoord>();
	}
	
	#endregion


	#region Instance Methods
	
	public void Fill(int radiusH, int radiusV, TData val) {
		for (int x = -radiusH; x <= radiusH; ++x) {
			for (int y = -radiusV; y <= radiusV; ++y) {
				GridCoord coord = new GridCoord(x, y);
				if (!Contains(coord)) Set(coord, val);
			}
		}
	}
	
	public bool Contains(GridCoord coord) {
		return m_data.ContainsKey(coord);
	}
	
	public TData Get(GridCoord coord) {
		TData result;
		m_data.TryGetValue(coord, out result);
		return result;
	}
	
	public TData GetFromPoint(Vector2 point) {
		return Get(PointToCoord(point));
	}
	
	public TData Set(GridCoord coord, TData val) {
		return m_data[coord] = val;
	}
	
	public TData Remove(GridCoord coord) {
		TData result = Get(coord);
		m_data.Remove(coord);
		return result;
	}
	
	public TData SetFromPoint(Vector2 point, TData val) {
		return Set(PointToCoord(point), val);
	}
	
	public bool ContainsElement(GridCoord coord) {
		return m_elements.ContainsKey(coord);
	}
	
	public IGridElement<TData> GetElement(GridCoord coord) {
		IGridElement<TData> result;
		m_elements.TryGetValue(coord, out result);
		return result;
	}
	
	public IGridElement<TData> GetElementFromPoint(Vector2 point) {
		return GetElement(PointToCoord(point));
	}
	
	public IGridElement<TData> AttachElement(GridCoord coord, IGridElement<TData> element) {
		if (AutoElementManagement) throw new Exception("Cannot manually attach element when AutoElementManagement is true.");
		
		return _attachElement(coord, element);
	}
	
	private IGridElement<TData> _attachElement(GridCoord coord, IGridElement<TData> element)
	{
		element.Grid = this;
		element.Coord = coord;
		element.Data = Get(coord);
		element.GridPos = CoordToPoint(coord);
		
		element.Activate();
		
		return m_elements[coord] = element;
	}
	
	public IGridElement<TData> DetachElement(GridCoord coord) {
		if (AutoElementManagement) throw new Exception("Cannot manually detach element when AutoElementManagement is true.");
		
		return _detachElement(coord);
	}
	
	public void ClearElements() {
		if (AutoElementManagement) throw new Exception("Cannot manually detach element when AutoElementManagement is true.");
		
		foreach (GridCoord coord in Coords()){
			if (ContainsElement(coord)) {
				_detachElement(coord).Destroy();
			}
		}
	}

	public IGridElement<TData> _detachElement(GridCoord coord) {
		IGridElement<TData> result = m_elements[coord];
		m_elements.Remove(coord);
		
		result.Deactivate();
		
		return result;
	}
	
	public Vector2 ToLocal(Vector2 globalPoint) {
		return ParentTransform.InverseTransformPoint(globalPoint);
	}
	
	public Vector2 ToGlobal(Vector2 localPoint) {
		return ParentTransform.TransformPoint(localPoint);
	}
	
	public IEnumerable Coords() {
		return m_data.Keys.ToArray();
	}
	
	public IEnumerable Data() {
		return m_data.Values;
	}

	public IEnumerable ElementCoords() {
		return m_elements.Keys;
	}

	public IEnumerable Elements() {
		return m_elements.Values;
	}
	
	public IEnumerable<GridCoord> Range(int radius, int spacing, GridCoord center) {
		for (int dx = -radius; dx <= radius; ++dx){
			for (int dy = -radius; dy <= radius; ++dy){
				yield return new GridCoord(
						center.x + dx * spacing,
						center.y + dy * spacing
						);
			}
		}
	}
	
	public IEnumerable<GridCoord> Line(GridCoord start, GridCoord end) {
		int distance = ManhattanDistance(start, end);
		
		for (int i = 0; i <= distance; ++i) {
			float xLerp = start.x + ((float)( end.x - start.x )) / distance * i;
			float yLerp = start.y + ((float)( end.y - start.y )) / distance * i;
			
			yield return new FloatGridCoord(xLerp, yLerp).Round();
		}
	}
	
	public IEnumerable<GridCoord> Neighbors(GridCoord coord) {
		foreach (GridCoord dir in GridCoord.DIRECTIONS) {
			yield return coord + dir;
		}
	}
	
	public IEnumerable<GridCoord> LineFromPoint(Vector2 from, Vector2 to, bool ignoreEndPoints = false) {
		FloatGridCoord start = PointToFloatCoord(from), end = PointToFloatCoord(to);

		int distance = ManhattanDistance(start.Round(), end.Round());
		
		for (int i = 1; i < distance; ++i) {
			float xLerp = start.x + ( end.x - start.x ) / distance * i;
			float yLerp = start.y + ( end.y - start.y ) / distance * i;
			
			yield return new FloatGridCoord(xLerp, yLerp).Round();
		}
		
		if (!ignoreEndPoints) {
			yield return start.Round();
			if (distance >= 1) yield return end.Round();
		}
	}
	
	private GridCoord _getClosestBatch(GridCoord coord) {
		int BatchDiameter = 1 + BatchRadius * 2;
		return new GridCoord(
				(int)(Math.Round((float)coord.x / BatchDiameter) * BatchDiameter),
				(int)(Math.Round((float)coord.y / BatchDiameter) * BatchDiameter)
				);
	}
	
	private void _addBatch(GridCoord batch) {
		for (int dx = -BatchRadius; dx <= BatchRadius; ++dx){
			for (int dy = -BatchRadius; dy <= BatchRadius; ++dy){
				GridCoord coord = new GridCoord(
						batch.x + dx,
						batch.y + dy
						);
				
				if (Contains(coord)) {
					IGridElement<TData> element = m_inactiveElements.Count == 0 ? this.ConstructElementFunc(this) : m_inactiveElements.Pop();
					_attachElement(coord, element);
				}
			}
		}
	}
	
	private void _removeBatch(GridCoord batch) {
		for (int dx = -BatchRadius; dx <= BatchRadius; ++dx){
			for (int dy = -BatchRadius; dy <= BatchRadius; ++dy){
				GridCoord coord = new GridCoord(
						batch.x + dx,
						batch.y + dy
						);
				
				if (Contains(coord)) {
					IGridElement<TData> element = _detachElement(coord);
					m_inactiveElements.Push(element);
				}
			}
		}
	}
	
	public void Update(Camera camera) {
		if (!AutoElementManagement) throw new Exception("Cannot update when AutoElementManagement is false.");
		
		Rect cameraRect = camera.pixelRect;
		float relativeDepth = ParentTransform.position.z - camera.transform.position.z;
		Vector2 camTopLeftLocal = ToLocal(Camera.main.ScreenToWorldPoint(new Vector3(cameraRect.x, cameraRect.y, relativeDepth)));
		Vector2 camBottomRightLocal = ToLocal(Camera.main.ScreenToWorldPoint(new Vector3(cameraRect.x + cameraRect.width, cameraRect.y + cameraRect.height, relativeDepth)));
		
		float batchDiameter = Util.Distance(this.TileSize, this.TileSize) * (1 + BatchRadius * 2);
			
		float maxViewRadius = (camBottomRightLocal - camTopLeftLocal).magnitude / 2.0f;
		int maxViewBatchRadius = (int)Math.Ceiling(maxViewRadius / batchDiameter) + 1;
		
		HashSet<GridCoord> lastActiveBatches = new HashSet<GridCoord>(m_activeBatches);
		
		// clear active batch
		m_activeBatches.Clear();
		
		// add active batch. for each batch that is not present last frame, add that shit.
		foreach (GridCoord batch in Range(
					maxViewBatchRadius,
					(1 + BatchRadius * 2),
					_getClosestBatch(PointToCoord((camTopLeftLocal + camBottomRightLocal) * 0.5f))
					)
				){
			m_activeBatches.Add(batch);
			if (!lastActiveBatches.Contains(batch)){
				_addBatch(batch);
			}
		}
		
		// remove any last frame batch that is not in active batch anymore.
		foreach (GridCoord batch in lastActiveBatches){
			if (!m_activeBatches.Contains(batch)){
				_removeBatch(batch);
			}
		}
		
		lastActiveBatches.Clear();
		
		if (m_inactiveElements.Count > m_elements.Count / 2) m_inactiveElements.Pop().Destroy();
	}
	
	public void RefreshElement(GridCoord coord) {
		if (ContainsElement(coord)) {
			GetElement(coord).Deactivate();
			GetElement(coord).Activate();
		}
	}
	
	/* public delegate void FloodFillCallback(GridCoord coord);
	public delegate void FloodFillCheck(GridCoord coord);
	
	public void FloodFill(GridCoord coord, FloodFillCheck check, FloodFillCallback callback)
	{
		
	} */
	
	public Vector2 CoordToPoint(GridCoord coord) {
		float x = ((float)coord.x) * this.TileSize;
		float y = ((float)coord.y) * this.TileSize;
		
		return new Vector2(x, y);
	}
	
	public float LeftEdge(GridCoord coord) {
		return this.CoordToPoint(coord).x - this.TileSize * 0.5f;
	}
	
	public float RightEdge(GridCoord coord) {
		return this.CoordToPoint(coord).x + this.TileSize * 0.5f;
	}
	
	public float TopEdge(GridCoord coord) {
		return this.CoordToPoint(coord).y + this.TileSize * 0.5f;
	}
	
	public float BottomEdge(GridCoord coord) {
		return this.CoordToPoint(coord).y - this.TileSize * 0.5f;
	}
	
	public static int ManhattanDistance(GridCoord a, GridCoord b) {
		return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
	}
	
	public FloatGridCoord PointToFloatCoord(Vector2 point) {
		float x = point.x / this.TileSize;
		float y = point.y / this.TileSize;
		
		return new FloatGridCoord(x, y);
	}
	
	public GridCoord PointToCoord(Vector2 point) {
		return PointToFloatCoord(point).Round();
	}
	
	public void ApplySampler(ArrayTexture2D sampler, int minX, int maxX, int minY, int maxY, bool useLinearFiltering, ApplySamplerDataFunction applyFunction, bool clamped = false) {
		Vector2 point = new Vector2();
		foreach (GridCoord coord in this.Coords()){
			point.x = Util.Map(coord.x, minX, maxX, 0.0f, 1.0f, clamped);
			point.y = Util.Map(coord.y, minY, maxY, 0.0f, 1.0f, clamped);
			float pixel = useLinearFiltering ? sampler.Texture2DLinear(point) : sampler.Texture2DNearest(point);
			applyFunction(this, coord, pixel);
		}
	} 

	#endregion
	
}

public static class Grid {
	public static Vector2 GetCollideOffset<T>(Grid<T> grid, Rect rect) where T : class, IGridCollidable {
		float top = rect.bottom, bottom = rect.top, left = rect.left, right = rect.right; // in world coordinate, rect.top is actually bottom.
		float topDelta = 0.0f, bottomDelta = 0.0f, leftDelta = 0.0f, rightDelta = 0.0f;
		Vector2 topLeft = new Vector2(rect.left, top);
		Vector2 topRight = new Vector2(rect.right, top);
		Vector2 bottomLeft = new Vector2(rect.left, bottom);
		Vector2 bottomRight = new Vector2(rect.right, bottom);
		T data;
		GridCoord cornerCoord;
		
		// top edge
		foreach (GridCoord coord in grid.LineFromPoint(topLeft, topRight, true)) {
			data = grid.Get(coord);
			if (data != null && data.Collidable) {
				topDelta = grid.BottomEdge(coord) - top;
				break;
			}
		}
		
		// right edge
		foreach (GridCoord coord in grid.LineFromPoint(topRight, bottomRight, true)) {
			data = grid.Get(coord);
			if (data != null && data.Collidable) {
				rightDelta = grid.LeftEdge(coord) - right;
				break;
			}
		}
		
		// bottom edge
		foreach (GridCoord coord in grid.LineFromPoint(bottomRight, bottomLeft, true)) {
			data = grid.Get(coord);
			if (data != null && data.Collidable) {
				bottomDelta = grid.TopEdge(coord) - bottom;
				break;
			}
		}
		
		// left edge
		foreach (GridCoord coord in grid.LineFromPoint(bottomLeft, topLeft, true)) {
			data = grid.Get(coord);
			if (data != null && data.Collidable) {
				leftDelta = grid.RightEdge(coord) - left;
				break;
			}
		} 
		
		// top left corner
		cornerCoord = grid.PointToCoord(topLeft);
		data = grid.Get(cornerCoord);
		if (data != null && data.Collidable) {
			if (Math.Abs(top - grid.BottomEdge(cornerCoord)) < Math.Abs(left - grid.RightEdge(cornerCoord))) {
				topDelta = grid.BottomEdge(cornerCoord) - top;
			}
			else {
				leftDelta = grid.RightEdge(cornerCoord) - left;
			}
		}
		
		// top right corner
		cornerCoord = grid.PointToCoord(topRight);
		data = grid.Get(cornerCoord);
		if (data != null && data.Collidable) {
			if (Math.Abs(top - grid.BottomEdge(cornerCoord)) < Math.Abs(right - grid.LeftEdge(cornerCoord))) {
				topDelta = grid.BottomEdge(cornerCoord) - top;
			}
			else {
				rightDelta = grid.LeftEdge(cornerCoord) - right;
			}
		}
		
		// bottom left corner
		cornerCoord = grid.PointToCoord(bottomLeft);
		data = grid.Get(cornerCoord);
		if (data != null && data.Collidable) {
			if (Math.Abs(bottom - grid.TopEdge(cornerCoord)) < Math.Abs(left - grid.RightEdge(cornerCoord))) {
				bottomDelta = grid.TopEdge(cornerCoord) - bottom;
			}
			else {
				leftDelta = grid.RightEdge(cornerCoord) - left;
			}
		}
		
		// bottom right corner
		cornerCoord = grid.PointToCoord(bottomRight);
		data = grid.Get(cornerCoord);
		if (data != null && data.Collidable) {
			if (Math.Abs(bottom - grid.TopEdge(cornerCoord)) < Math.Abs(right - grid.LeftEdge(cornerCoord))) {
				bottomDelta = grid.TopEdge(cornerCoord) - bottom;
			}
			else {
				rightDelta = grid.LeftEdge(cornerCoord) - right;
			}
		}
		
		return new Vector2(leftDelta + rightDelta, topDelta + bottomDelta);
	}
}

