using UnityEngine;

public interface IGridElement<TData> where TData : class {
	Grid<TData> Grid{get; set;}
	GridCoord Coord{get; set;}
	Vector2 GridPos{get; set;}
	TData Data{get; set;}
	
	void Activate();
	void Deactivate();
	void Destroy();
}
