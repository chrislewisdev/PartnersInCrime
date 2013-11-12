using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RobotMovement))]
public class RobotController : AiControllable {
	
	private tk2dSpriteAnimator animations;
	private float health = 2f;
	private float timeSinceLastHit = 5f;
	private RobotMovement movement;
	private bool poweredUp = false;
	private float runTimer = 0f;
	private ParticleSystem smokeParticles;
	private bool destroyed = false;
	private bool punching = false;
	
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
		smokeParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
		if (!smokeParticles)
			Debug.LogError("Robot does not have smoke particles as a child");
		else
			smokeParticles.enableEmission = false;
	}
	
	public void Damage()
	{
		health -= 1f;
		
		if (health < 0f && !destroyed)
		{
			//Application.LoadLevel (Application.loadedLevelName);
			animations.Play (animations.Library.GetClipByName ("Death"));
			Instantiate(Resources.Load("Destroyed") as GameObject);
			Instantiate(Resources.Load("Explosion") as GameObject, transform.position, Quaternion.identity);
			//renderer.enabled = false;
			//GetComponent<CharacterController>().enabled = false;
			destroyed = true;
		}
		else
		{
			Flash (Color.red);
			timeSinceLastHit = 0f;
		}
	}
	
	public void Flash(Color colour)
	{
		GetComponent<SpriteEffects>().FlashColour (colour, 0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (destroyed) return;
		
		if (Input.GetButtonDown("Attack"))
		{
			attack();
			animations.Play (animations.Library.GetClipByName("Punch"));
			punching = true;
		}
		
		if (punching)
		{
			if (!animations.Playing || animations.CurrentClip.name != "Punch")
				punching = false;
		}

		timeSinceLastHit += Time.deltaTime;
		
		if (health < 10f && timeSinceLastHit < 3f) health += Time.deltaTime * 2f;
		
		movement.UpdateMovement ();
		
		//Update sprite
		if (movement.Velocity.y > 0 && movement.AttachedToLadder)
		{
			animations.Play (animations.Library.GetClipByName("Ladder_Climb_Up"));
		}
		else if (movement.Velocity.y < 0 && movement.AttachedToLadder)
		{
			animations.Play (animations.Library.GetClipByName("Ladder_Climb_Down"));
		}
		else if (movement.Velocity.y > 0)
		{
			if (animations.CurrentClip.name != "Jump_Start")
				animations.Play (animations.Library.GetClipByName ("Jump_Start"));
		}
		else if (movement.Velocity.y < 0)
		{
			if (animations.CurrentClip.name != "Jump_End")
				animations.Play (animations.Library.GetClipByName ("Jump_End"));
		}
		else if (movement.Velocity.x != 0)
		{
			if (movement.Velocity.x > 0) animations.Sprite.FlipX = false;
			else animations.Sprite.FlipX = true;
			animations.Play(animations.Library.GetClipByName("Run"));
		}
		else if (!punching)
		{
			animations.Play (animations.Library.GetClipByName("Idle"));
		}
		
		// Smoke particle effect when running
		if (smokeParticles)
		{
			if (animations.CurrentClip.name == "Run")
			{
				smokeParticles.enableEmission = true;
			}
			else
				smokeParticles.enableEmission = false;
		}
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
