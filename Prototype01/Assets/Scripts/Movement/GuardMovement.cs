using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class GuardMovement : MonoBehaviour {
	
	public float walkSpeed;
	public PatrolPath patrolPath;
	public tk2dTileMap ladderMap;
	
	private CharacterController moveController;
	private int pathIndex = 0;
	private Vector3 velocity = Vector3.zero;
	public Vector3 Velocity { get { return velocity; } }
	private float sleepingCounter = 0;
	private int walkDirection = 1;

	// Use this for initialization
	void Start () 
	{
		moveController = GetComponent<CharacterController>();
		ladderMap = GameManager.gameManager.ladderMap;
		if (ladderMap == null)
			Debug.LogError("ladder map was not set for " + gameObject.name);
	}
	
	// Update is called once per frame
	public void UpdateMovement() 
	{
		if (sleepingCounter > 0) sleepingCounter -= Time.deltaTime;
		
		if (patrolPath != null && patrolPath.Size () > 0 && sleepingCounter <= 0 && IsWaypointReachable(patrolPath[pathIndex])) 
			FollowPath();
		else if (sleepingCounter <= 0) 
			WalkToEdges();
	}
	
	private void FollowPath()
	{
		Vector3 target = patrolPath[pathIndex];
		
		//Utility.LogChangedValue("ReachedWaypoint", ReachedWaypoint (target));
		
		velocity = moveController.velocity;
		
		velocity.x = target.x - transform.position.x;
		velocity.x = Mathf.Sign (velocity.x) * walkSpeed;
		walkDirection = (int)Mathf.Sign (velocity.x);
		
		if (ladderMap == null || ladderMap.GetTileIdAtPosition(transform.position, 0) == -1)
		{
			velocity += Physics.gravity * Time.deltaTime;
			if (velocity.y < Physics.gravity.y)
			{
				velocity.y = Physics.gravity.y;
			}
		}
		else
		{
			velocity.y = 0f;
		}
		
		moveController.Move (velocity * Time.deltaTime);
		
		if (ReachedWaypoint (target))
		{
			pathIndex = patrolPath.GetNextWaypointIndex(pathIndex);
			Sleep (1);
		}
	}
	
	private void WalkToEdges()
	{
		velocity.x = walkSpeed * walkDirection;
		
		if (ladderMap == null || ladderMap.GetTileIdAtPosition(transform.position, 0) == -1)
		{
			velocity += Physics.gravity * Time.deltaTime;
			if (velocity.y < Physics.gravity.y)
			{
				velocity.y = Physics.gravity.y;
			}
		}
		else
		{
			velocity.y = 0f;
		}
		
		moveController.Move (velocity * Time.deltaTime);
		
		if (FacingEdge() || (moveController.collisionFlags & CollisionFlags.Sides) != 0)
		{
			walkDirection *= -1;
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
	
	private bool IsWaypointReachable(Vector3 target)
	{
		Vector3 reachablePosition = new Vector3(target.x, transform.position.y, transform.position.z);
		
		float distance = Vector2.Distance (reachablePosition, target);
		
		//Check if the distance is less than the size of our bounds
		return (distance < moveController.bounds.size.magnitude / 2);
	}
	
	private bool FacingEdge()
	{
		Vector3 halfHeight = Vector3.down * collider.bounds.size.y / 1.5f;
		Vector3 direction = Vector3.left * -walkDirection * collider.bounds.size.x / 1.5f;
		float distance = (halfHeight + direction).magnitude;
		Ray ray = new Ray(transform.position, (halfHeight + direction).normalized);
		
		Debug.DrawRay (transform.position, (halfHeight + direction).normalized * distance, Color.red);
		RaycastHit info;
		
		if (!Physics.Raycast (ray, out info, distance))
		{
			if (ladderMap == null) return true;
			int tileId = ladderMap.GetTileIdAtPosition (transform.position + (halfHeight + direction).normalized * distance, 0);
			if (tileId == -1)
				return true;
		}
		
		return false;
	}
	
	public void Sleep(float seconds)
	{
		sleepingCounter = seconds;
	}
}
