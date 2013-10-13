using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RobotMovement))]
public class RobotController : AiControllable {
	
	private tk2dSpriteAnimator animations;
	private float timeSinceLastHit = 5f;
	private RobotMovement movement;
	
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
		RaycastHit hit;
		Debug.Log("Punch");
		if (Physics.Raycast(new Ray(transform.position, new Vector3(animations.Sprite.FlipX ? 0.0f : 1.0f, -.5f, 0.0f)), out hit))
		{
			if (hit.distance < 1.5f)
			{
				GuardController guard = hit.collider.GetComponent<GuardController>();
				if (guard != null)
				{
					if (guard.isPossessed())
						guard.Damage(10);
				}
			}
		}
	}
}
