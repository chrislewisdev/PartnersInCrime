using UnityEngine;
using System.Collections;

/// <summary>
/// RobotMovement encapsulates all the logic behind the Robot Player's movement.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class RobotMovement : MonoBehaviour {
	
	//Editor variables
	public float speed;
	public float jumpStrength;
	public float gravityFactor;
	public tk2dTileMap ladderMap;
	
	private CharacterController moveController;
	private Vector3 velocity = Vector3.zero;
	public Vector3 Velocity { get { return velocity; } }
	private bool jumped = false;
	private bool attachedToLadder = false;
	private bool nextToLadder;

	// Use this for initialization
	void Start () {
		moveController = GetComponent<CharacterController>();
		
		if (ladderMap == null)
		{
			Debug.LogWarning ("No Ladder Map set for the Robot Player. He will not be able to climb any ladders" +
				"until he is provided a tilemap.");
		}
	}
	
	// Update is called once per frame
	public void UpdateMovement()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		float jump = Input.GetAxis ("Jump");
		
		//Always set horizontal velocity exactly to requested speed
		velocity.x = horizontal * speed;
		
		//Detach from ladders when 
		if (jump > 0) attachedToLadder = false;
		
		//Check if we are currently next to a ladder
		if (ladderMap != null)
		{
			nextToLadder = (ladderMap.GetTileIdAtPosition (transform.position, 0) != -1);
		}
		else
		{
			nextToLadder = false;
		}
		
		//Attach to a ladder only if next to one and the player is requesting vertical movement
		if (vertical != 0 && nextToLadder)
		{
			attachedToLadder = true;
			velocity.y = 0;
		}
		if (!nextToLadder) attachedToLadder = false;
		
		//If the player is attached to a ladder and moving, do so but only if that won't move them outside the ladder
		if (attachedToLadder)
		{
			velocity.y = vertical * speed;
			//If moving in this direction would take us away from a ladder, cancel it
			if (ladderMap.GetTileIdAtPosition (transform.position + velocity * Time.deltaTime, 0) == -1)
			{
				velocity.y = 0;
			}
		}
		
		//Start a jump only when a jump has been pressed initially
		if (jump > 0 && !jumped)
		{
			if (moveController.isGrounded || attachedToLadder)
			{
				velocity += Vector3.up * jumpStrength;
			}
			jumped = true;
		}
		else if (jump <= 0)
		{
			jumped = false;
		}
		
		//Apply gravity only if not attached to a ladder
		if (!attachedToLadder)
		{
			velocity += Physics.gravity * Time.deltaTime * gravityFactor;
			if (velocity.y < Physics.gravity.y)
			{
				velocity.y = Physics.gravity.y;
			}
		}
		
		PerformMove ();
	}
	
	//Performs the actual Character Controller move and update some of our velocity according to the returned
	//collision info.
	private void PerformMove()
	{
		CollisionFlags collision = moveController.Move (velocity * Time.deltaTime);
		if ((collision & CollisionFlags.Above) != 0 && velocity.y > 0)
		{
			velocity.y = 0;
		}
		if ((collision & CollisionFlags.Below) != 0 && velocity.y < 0)
		{
			velocity.y = 0;
		}
	}
}
