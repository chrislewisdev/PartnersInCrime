using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//PatrolPath is used to represent groups of waypoints in the game. Create a game-object with multiple
//PatrolPathWaypoints childed to it, and PatrolPath will detect them all. 
//It provides various functions for retrieving info on the patrol path in general.
public class PatrolPath : MonoBehaviour {
	
	private Dictionary<int, PatrolPathWaypoint> waypoints = new Dictionary<int, PatrolPathWaypoint>();

	// Use this for initialization
	public void Start () 
	{
		//Find all PatrolPathWaypoints attached to this object
		foreach (PatrolPathWaypoint waypoint in GetComponentsInChildren<PatrolPathWaypoint>())
		{
			AddWaypoint(waypoint);
		}
	}
	
	/// <summary>
	/// Adds a new waypoint to our path and orders the path accordingly.
	/// </summary>
	/// <param name='waypoint'>
	/// Waypoint.
	/// </param>
	public void AddWaypoint(PatrolPathWaypoint waypoint)
	{
		//Make sure duplicate sequence ordering is not entered
		if (waypoints.ContainsKey(waypoint.orderInSequence))
		{
			Debug.LogError ("Duplicate order no. in PatrolPathWaypoint on object: " + waypoint.gameObject.name);
			return;
		}
		
		waypoints.Add (waypoint.orderInSequence, waypoint);
		
		ValidateWaypoints();
	}
	
	/// <summary>
	/// Performs miscellaneous validation on waypoints to make sure everything is as expected.
	/// </summary>
	private void ValidateWaypoints()
	{
		//Create a copy of our waypoint keys so we can sort it and validate sequencing
		List<int> waypointKeys = new List<int>(waypoints.Keys);
		
		waypointKeys.Sort ();
		
		//Once our waypoints are sorted, a valid sequence should be a perfect number sequence, e.g.
		//0, 1, 2, 3, etc... so if the values don't match our iterator variable, it means we have a gap in ordering
		for (int i = 0; i < waypointKeys.Count; i++)
		{
			if (waypointKeys[i] != i)
			{
				Debug.LogError(gameObject.name + ": Waypoint ordering is not sequential!");
			}
		}
	}
	
	/// <summary>
	/// Returns the no. of waypoints in this path.
	/// </summary>
	public int Size()
	{
		return waypoints.Count;
	}
	
	//Index operator
	public Vector3 this[int index]
	{
		get
		{
			return waypoints[index].transform.position;
		}
	}
	
	/// <summary>
	/// Gets the index of the next waypoint, based off the current index specified by the caller.
	/// Wraps the index back around to the beginning if it's at the end.
	/// </summary>
	/// <returns>
	/// The next waypoint index.
	/// </returns>
	/// <param name='currentIndex'>
	/// Current index.
	/// </param>
	public int GetNextWaypointIndex(int currentIndex)
	{
		if (currentIndex + 1 >= Size ())
		{
			return 0;
		}
		else return currentIndex + 1;
	}
}
