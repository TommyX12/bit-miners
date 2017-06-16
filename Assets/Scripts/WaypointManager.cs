using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointManager : MyMono {
	public static WaypointManager Current;
	
	public static float RemoveRadius = 0.25f;
	
	public static readonly int NumDefaultWaypoints = 10;
	public static readonly KeyCode[] DefaultBindings = new KeyCode[]{
		KeyCode.Alpha0,
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
	};

	public GameObject WaypointPrefab;
	
	private static Dictionary<string, Waypoint> waypoints = new Dictionary<string, Waypoint>();
	// private static Dictionary<GridCoord, Waypoint> placedWaypoints = new Dictionary<GridCoord, Waypoint>();

	void Awake() {
		Current = this;
		
		for (int i = 0; i < NumDefaultWaypoints; ++i) {
			string name = i.ToString();
			waypoints[name] = new Waypoint(name, DefaultBindings[i]);
		}
	}

	public override void PausingUpdate() {
		base.PausingUpdate();
		
		foreach (Waypoint waypoint in this.Waypoints()) {
			if (Input.GetKeyDown(waypoint.KeyBinding)) {
				Vector2 mousePos = Util.CameraToWorld(Camera.main, Input.mousePosition, 0);
				// GridCoord coord = Map.Current.Grid.PointToCoord(pos);
				if (waypoint.Placed && Util.InRange(waypoint.Pos, mousePos, RemoveRadius)) {
					waypoint.Remove();
				}
				else {
					waypoint.Place(mousePos);
				}
				break;
			}
		}
	}
	
	public Waypoint GetWaypoint(string name) {
		Waypoint waypoint;
		waypoints.TryGetValue(name, out waypoint);
		return waypoint;
	}
	
	/* public Waypoint GetWaypoint(GridCoord coord) {
		Waypoint waypoint;
		placedWaypoints.TryGetValue(coord, out waypoint);
		return waypoint;
	} */
	
	public void PlaceWaypoint(string name, Vector2 pos) {
		waypoints[name].Place(pos);
	}
	
	public void RemoveWaypoint(string name) {
		waypoints[name].Remove();
	}
	
	/* public void RemoveWaypoint(GridCoord coord) {
		if (placedWaypoints.ContainsKey(coord)) {
			placedWaypoints[coord].Remove();
		}
	} */
	
	public void ClearWaypoints() {
		foreach (Waypoint waypoint in this.Waypoints()) {
			waypoint.Remove();
		}
	}
	
	public IEnumerable Waypoints() {
		return waypoints.Values;
	}
	
	/* public IEnumerable PlacedWaypoints() {
		return placedWaypoints.Values;
	} */
	
	public class Waypoint {
		
		public GameObject View {
			get; private set;
		}
		public KeyCode KeyBinding {
			get; private set;
		}
		public string Name {
			get; private set;
		}
		public bool Placed {
			get; private set;
		}
		public Vector2 Pos {
			get; private set;
		}
		
		public Waypoint(string name, KeyCode keyBinding) {
			this.View = GameObject.Instantiate(WaypointManager.Current.WaypointPrefab);
			this.View.GetComponentInChildren<Text>().text = name;
			this.View.SetActive(false);
			this.Name = name;
			this.KeyBinding = keyBinding;
			this.Placed = false;
		}
		
		public void Place(Vector2 pos) {
			this.Placed = true;
			this.Pos = pos;
			this.View.SetActive(true);
			this.View.transform.position = pos;
		}
		
		public void Remove() {
			if (!this.Placed) return;
			
			this.Placed = false;
			this.View.SetActive(false);
		}
		
	}
}
