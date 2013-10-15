using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RobotMovement))]
public class RobotController : AiControllable {
	
	private tk2dSpriteAnimator animations;
	private float timeSinceLastHit = 5f;
	private RobotMovement movement;
	private bool poweredUp = false;
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	public override void activateGadget (bool triggeredByAi)
	{
	}
	
	public override void aiArrived ()
	{
	}
	
	public override void aiLeft ()
	{
	}
	
	//Called when robot retrieves prize, allows it to punch robots and breakable doors
	public void powerUpRobot()
	{
		poweredUp = true;
	}
	
	// Use this for initialization
	void Start () {
		animations = GetComponent<tk2dSpriteAnimator>();
		movement = GetComponent<RobotMovement>();
	}
	
	public void Damage()
	{
		if (timeSinceLastHit < 5f)
		{
			Application.LoadLevel (Application.loadedLevelName);
		}
		else
		{
			timeSinceLastHit = 0f;
			Flash (Color.red);
		}
	}
	
	public void Flash(Color colour)
	{
		GetComponent<SpriteEffects>().FlashColour (colour, 0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Attack"))
			attack();


			
		if (timeSinceLastHit < 5f) timeSinceLastHit += Time.deltaTime;
		
		movement.UpdateMovement ();
		
		//Update sprite
		if (movement.Velocity.x != 0)
		{
			if (movement.Velocity.x > 0) animations.Sprite.FlipX = false;
			else animations.Sprite.FlipX = true;
			animations.Play(animations.Library.GetClipByName("Run"));
		}
		else
		{
			animations.Play (animations.Library.GetClipByName("Idle"));
		}
		if (movement.Velocity.y > 0 && animations.CurrentClip.name != "Jump_Start")
		{
			animations.Play (animations.Library.GetClipByName ("Jump_Start"));
		}
		else if (movement.Velocity.y < 0)
		{
			animations.Play (animations.Library.GetClipByName ("Jump_End"));
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit col)
	{
		// UGLY workaround code, don't look at me!! 
		if (col.collider.gameObject.GetComponent<MovingPlatform>() != null)
			col.collider.gameObject.GetComponent<MovingPlatform>().characterCollision(gameObject);
	}
	
	//Attack robot for takedown
	void attack()
	{
		//seriously ugly but it should be a problem for performance
		GuardController[] guardControllers = (GuardController[])Object.FindObjectsOfType(typeof(GuardController));
		
		foreach (GuardController guard in guardControllers)
		{
			if (Vector3.Distance(transform.position, guard.transform.position) < 5f)
			{
				if (guard.isPossessed() || poweredUp)
				{
					guard.Damage(10);
				}
			}
		}
		
		BreakableDoor[] doors = (BreakableDoor[])Object.FindObjectsOfType(typeof(BreakableDoor));
		foreach (BreakableDoor door in doors)
		{
			if (Vector3.Distance(transform.position, door.transform.position) < 5f)
			{
				if (poweredUp)
				{
					door.breakDoor();
				}
			}
		}
	}
}
