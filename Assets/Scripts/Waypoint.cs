using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MyMono {
    public static GameObject[] Waypoints = new GameObject[10];
    public static bool[] Exists = new bool[10] { false, false, false, false, false, false, false, false, false, false };

    public static void SetWaypoint(int waypoint, GridCoord pos) {
        if (waypoint > 9) {
            return;
        }

        Waypoints[waypoint].SetActive(true);
        Waypoints[waypoint].transform.position = Map.Current.Grid.CoordToPoint(pos);
        Exists[waypoint] = true;
    }

    public GameObject WaypointPrefab;

    void Start()
    {
        for (int i = 0; i < 10; i++) {
            Waypoints[i] = GameObject.Instantiate(WaypointPrefab);
            Waypoints[i].GetComponentInChildren<Text>().text = (i).ToString();
            Waypoints[i].SetActive(false);
        }
    }

    public override void PausingUpdate()
    {
        base.PausingUpdate();
        Vector2 pos = Util.CameraToWorld(Camera.main, Input.mousePosition, 0);
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Waypoint.SetWaypoint(1, Map.Current.Grid.PointToCoord(pos));
            } else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Waypoint.SetWaypoint(2, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Waypoint.SetWaypoint(3, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Waypoint.SetWaypoint(4, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Waypoint.SetWaypoint(5, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Waypoint.SetWaypoint(6, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Waypoint.SetWaypoint(7, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Waypoint.SetWaypoint(8, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Waypoint.SetWaypoint(9, Map.Current.Grid.PointToCoord(pos));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Waypoint.SetWaypoint(0, Map.Current.Grid.PointToCoord(pos));
            }
    }
}
