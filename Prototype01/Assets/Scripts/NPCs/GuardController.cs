using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(ReactionLogic))]
[RequireComponent(typeof(ReactionMethod))]
public class GuardController : AiControllable {
	
	public float walkSpeed;
	public PatrolPath patrolPath;
	
	private CharacterController moveController;
	private FieldOfView sight;
	private ReactionLogic reaction;
	private ReactionMethod reactionMethod;
	private int pathIndex = 0;
	private Vector3 velocity = Vector3.zero;
	private float sleepingCounter = 0;
	private int walkDirection = 1;
	Alertness alertness = Alertness.Normal;
	private bool isActive;
	private int health = 3;
	
	public override void aiArrived ()
	{
		isActive = false;
		sight.enabled = false;
		sight.getLight().LightEnabled = false;
	}
	
	public override void aiLeft ()
	{
		isActive = true;
		sight.enabled = true;
		sight.getLight().LightEnabled = true;
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	public override void activateGadget(bool triggeredByAi)
	{
	}
	
	public void Damage(int amount)
	{
		health -= amount;
		if (health <= 0)
		{
			if (isPossessed())
				GameManager.gameManager.AI.occupiedGadgetDestroyed();
			Destroy(gameObject);
		}
	}
	
	// Use this for initialization
	void Start () {
		moveController = GetComponent<CharacterController>();
		sight = GetComponent<FieldOfView>();
		reaction = GetComponent<ReactionLogic>();
		reactionMethod = GetComponent<ReactionMethod>();
		isActive = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isActive) // Do nothing if currently occupied by ai
			return;
		
		if (sleepingCounter > 0) sleepingCounter -= Time.deltaTime;
		
		if (sight.IsObjectInView (GameManager.gameManager.Robot.gameObject))
			reaction.OnIntruderInSight();
		else
		{
			reaction.OnIntruderOutOfSight();
		}
		
		alertness = reaction.DetermineAlertness();
		if (alertness == Alertness.Aggressive)
		{
			reactionMethod.OnAggressive();
			activateConnectedGadgets();
			//Return so we don't move
			return;
		}
		else if (alertness == Alertness.Suspicious)
		{
			reactionMethod.OnSuspicious();
			//Don't move if intruder is still in sight
			if (sight.IsObjectInView (GameManager.gameManager.Robot.gameObject)) return;
		}
		else if (alertness == Alertness.Normal)
		{
			reactionMethod.OnNormal ();
		}
		
		if (patrolPath != null && patrolPath.Size () > 0 && sleepingCounter <= 0) FollowPath();
		else if (sleepingCounter <= 0) WalkToEdges();
	}
	
	private void FollowPath()
	{
		Vector3 target = patrolPath[pathIndex];
		
		//Utility.LogChangedValue("ReachedWaypoint", ReachedWaypoint (target));
		
		velocity = moveController.velocity;
		
		velocity.x = target.x - transform.position.x;
		velocity.x = Mathf.Sign (velocity.x) * walkSpeed;
		
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
	
	private void WalkToEdges()
	{
		velocity.x = walkSpeed * walkDirection;
		
		//Set our FOV orientation
		sight.Rotation = Mathf.Sign (velocity.x) == 1 ? 0 : 180;
		
		velocity += Physics.gravity * Time.deltaTime;
		if (velocity.y < Physics.gravity.y)
		{
			velocity.y = Physics.gravity.y;
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
	
	private bool FacingEdge()
	{
		Vector3 halfHeight = Vector3.down * collider.bounds.size.y / 1.5f;
		Vector3 direction = Vector3.left * -walkDirection * collider.bounds.size.x / 1.5f;
		float distance = (halfHeight + direction).magnitude;
		Ray ray = new Ray(transform.position, (halfHeight + direction).normalized);
		
		Debug.DrawRay (transform.position, halfHeight + direction, Color.red);
		RaycastHit info;
		
		if (Physics.Raycast (ray, out info, distance))
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	public void Sleep(float seconds)
	{
		sleepingCounter = seconds;
	}
}
