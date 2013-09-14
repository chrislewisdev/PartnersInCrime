using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FieldOfView))]
public class GuardController : MonoBehaviour {
	
	public PatrolPath patrolPath;
	
	private CharacterController moveController;
	private FieldOfView sight;
	private int pathIndex = 0;
	private Vector3 velocity = Vector3.zero;
	private float sleepingCounter = 0;

	// Use this for initialization
	void Start () {
		if (patrolPath == null)
		{
			Debug.LogWarning ("Guard '" + gameObject.name + "' has no patrol path! Standing around dumbly...");
		}
		
		moveController = GetComponent<CharacterController>();
		sight = GetComponent<FieldOfView>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (sleepingCounter > 0) sleepingCounter -= Time.deltaTime;
		if (patrolPath.Size () > 0 && sleepingCounter <= 0) FollowPath();
	}
	
	private void FollowPath()
	{
		Vector3 target = patrolPath[pathIndex];
		
		//Utility.LogChangedValue("ReachedWaypoint", ReachedWaypoint (target));
		
		velocity = moveController.velocity;
		
		velocity.x = target.x - transform.position.x;
		velocity.x = Mathf.Sign (velocity.x) * 5;
		
		//Set our FOV orientation
		sight.Rotation = Mathf.Sign (velocity.x) == 1 ? 0 : 180;
		
		velocity += Physics.gravity * Time.deltaTime;
		if (velocity.y < Physics.gravity.y)
		{
			velocity.y = Physics.gravity.y;
		}
		
		moveController.Move (velocity * Time.deltaTime);
		
		if (ReachedWaypoint (target))
		{
			pathIndex = patrolPath.GetNextWaypointIndex(pathIndex);
			Sleep (1);
		}
	}
	
	/// <summary>
	/// Checks if we've reached our current waypoint.
	/// </summary>
	/// <returns>
	/// The waypoint.
	/// </returns>
	/// <param name='target'>
	/// If set to <c>true</c> target.
	/// </param>
	private bool ReachedWaypoint(Vector3 target)
	{
		//Calculate the distance- note that the use of Vector2 here is INTENTIONAL, to ignore the Z axis!
		float distance = Vector2.Distance (transform.position, target);
		
		//Check if the distance is less than the size of our bounds
		return (distance < moveController.bounds.size.magnitude / 2);
	}
	
	private void Sleep(float seconds)
	{
		sleepingCounter = seconds;
	}
}
