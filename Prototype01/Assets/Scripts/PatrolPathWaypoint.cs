using UnityEngine;
using System.Collections;

//This class is used as a tagging class for waypoints in a PatrolPath. Any instance of this class
//must be placed on a (generally empty) game object that is childed to a PatrolPath object-
//the PatrolPath will automatically detect all waypoints attached to it.
public class PatrolPathWaypoint : MonoBehaviour {
	
	//Use this to set what no. this point is in the overall sequence of waypoints
	public int orderInSequence;
}
