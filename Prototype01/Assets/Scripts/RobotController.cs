using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class RobotController : GadgetControllerInterface {
	
	//Editor variables
	public float speed;
	public float jumpStrength;
	public float gravityFactor;
	public LayerMask ladderMask;
	public tk2dTileMap ladderMap;
	
	CharacterController moveController;
	Vector3 velocity = Vector3.zero;
	private bool jumped = false;
	private tk2dSpriteAnimator animations;
	private GameObject attachedLadder = null;
	private bool attachedToLadder = false;
	private float timeSinceLastHit = 0f;
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	public override void aiSendInput (ButtonState buttonState)
	{
	}

	// Use this for initialization
	void Start () {
		moveController = GetComponent<CharacterController>();
		animations = GetComponent<tk2dSpriteAnimator>();
	}
	
	public void Damage()
	{
		if (timeSinceLastHit < 5f)
		{
			Debug.LogError ("GameOver");
		}
		else
		{
			timeSinceLastHit = 0f;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timeSinceLastHit < 5f) timeSinceLastHit += Time.deltaTime;
		
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		float jump = Input.GetAxis ("Jump");
		
		//velocity = Physics.gravity;
		//velocity = moveController.velocity;
		velocity.x = horizontal * speed;
		
		if (jump > 0) attachedToLadder = false;
	
		if (attachedToLadder)
		{
			//velocity.x = 0;
		}
		
		RaycastHit info;
		bool nextToLadder;
		if (ladderMap != null)
			nextToLadder = (ladderMap.GetTileIdAtPosition (transform.position, 0) != -1);
		else
			nextToLadder = false;
		Debug.DrawRay (transform.position, Vector3.forward * 10f, Color.yellow);
		Utility.LogChangedValue("NextToLadder", nextToLadder);
		
		if (vertical != 0 && nextToLadder)
		{
			//attachedLadder = info.collider.gameObject;
			//Vector3 newPosition = transform.position;
			//newPosition.x = attachedLadder.transform.position.x;
			//newPosition.x = info.collider.bounds.center.x;
			//transform.position = newPosition;
			attachedToLadder = true;
			velocity.y = 0;
		}
		if (!nextToLadder) attachedToLadder = false;
		
		if (attachedToLadder)
		{
			velocity.y = vertical * speed;
			//If moving in this direction would take us away from a ladder, cancel it
			//if (!Physics.Raycast(new Ray(transform.position + velocity * Time.deltaTime, Vector3.forward), 10f, ladderMask))
			if (ladderMap.GetTileIdAtPosition (transform.position + velocity * Time.deltaTime, 0) == -1)
			{
				velocity.y = 0;
			}
		}
		
		if (jump > 0 && !jumped)
		{
			if (moveController.isGrounded)
			{
				velocity += Vector3.up * jumpStrength;
			}
			jumped = true;
		}
		else if (jump <= 0)
		{
			jumped = false;
		}
		
		if (!attachedToLadder)
		{
			velocity += Physics.gravity * Time.deltaTime * gravityFactor;
			if (velocity.y < Physics.gravity.y)
			{
				velocity.y = Physics.gravity.y;
			}
		}
		
		CollisionFlags collision = moveController.Move (velocity * Time.deltaTime);
		if ((collision & CollisionFlags.Above) != 0 && velocity.y > 0)
		{
			velocity.y = 0;
		}
		if ((collision & CollisionFlags.Below) != 0 && velocity.y < 0)
		{
			velocity.y = 0;
		}
		
		//Update sprite
		if (velocity.x != 0)
		{
			if (velocity.x > 0) animations.Sprite.FlipX = false;
			else animations.Sprite.FlipX = true;
			animations.Play(animations.Library.GetClipByName("Run"));
		}
		else
		{
			animations.Play (animations.Library.GetClipByName("Idle"));
		}
		if (velocity.y > 0)
		{
			animations.Play (animations.Library.GetClipByName ("Jump_Start"));
		}
		else if (velocity.y < 0)
		{
			animations.Play (animations.Library.GetClipByName ("Jump_End"));
		}
	}
}
