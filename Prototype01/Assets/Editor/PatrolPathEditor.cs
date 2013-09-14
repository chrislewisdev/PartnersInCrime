using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PatrolPath))]
public class PatrolPathEditor : Editor {

	public override void OnInspectorGUI()
	{
		PatrolPath path = (PatrolPath)target;
		
		//Add a button that will add new waypoints
		if (GUILayout.Button ("Add Waypoint"))
		{
			//Call Start() on the path so its waypoint data is initialised
			path.Start ();
			
			//Create the new object to house our waypoint
			GameObject newWaypoint = new GameObject();
			newWaypoint.name = "Waypoint" + path.Size ();
			newWaypoint.transform.parent = path.transform;
			newWaypoint.transform.localPosition = Vector3.zero;
			//Create the waypoint component instance
			PatrolPathWaypoint waypoint = newWaypoint.AddComponent<PatrolPathWaypoint>();
			waypoint.orderInSequence = path.Size ();
			//Add it to the path!
			path.AddWaypoint(waypoint);
		}
	}
}
