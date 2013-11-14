using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GuardMovement))]
[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(ReactionLogic))]
[RequireComponent(typeof(ReactionMethod))]
[RequireComponent(typeof(SpriteEffects))]
public class GuardController : AiControllable {
	
	public AudioClip alertSound;
	public AudioClip suspiciousSound;
	
	private FieldOfView sight;
	private ReactionLogic reaction;
	private ReactionMethod reactionMethod;
	Alertness alertness = Alertness.Normal;
	Alertness previousAlertness = Alertness.Normal;
	private bool isActive;
	private int health = 3;
	private GuardMovement movement;
	private tk2dSpriteAnimator animations;
	private SpriteEffects effects;
	private int orientation = 1;
	private const float bumpRange = 6f;
	private bool destroyed = false;
	private GameObject gunPosition;
	private float stunTimer = 0;
	private ParticleSystem stunSparks;
	
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
		effects.FlashColour (Color.red, 0.5f);
		if (health <= 0)
		{
			if (isPossessed())
				GameManager.gameManager.AI.occupiedGadgetDestroyed();
			animations.Play (animations.Library.GetClipByName("Death"));
			Invoke ("Explode", 1.0f);
			destroyed = true;
		}
	}
	
	private void Explode()
	{
		Destroy(gameObject);
		Instantiate(Resources.Load("Explosion") as GameObject, transform.position, Quaternion.identity);
	}
	
	// Use this for initialization
	void Start () {
		sight = GetComponent<FieldOfView>();
		reaction = GetComponent<ReactionLogic>();
		reactionMethod = GetComponent<ReactionMethod>();
		isActive = true;
		movement = GetComponent<GuardMovement>();
		animations = GetComponent<tk2dSpriteAnimator>();
		effects = GetComponent<SpriteEffects>();
		gunPosition = GetComponentInChildren<PositionMarker>().gameObject;
		stunSparks = GetComponentInChildren<ParticleSystem>();
		stunSparks.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (stunTimer > 0) stunTimer -= Time.deltaTime;
		
		if (!isActive || destroyed) // Do nothing if currently occupied by ai
			return;
		
		UpdateVisuals();
		
		if (sight.IsObjectInView (GameManager.gameManager.Robot.gameObject)
			|| Vector3.Distance(transform.position, GameManager.gameManager.Robot.transform.position) < bumpRange)
		{
			reaction.OnIntruderInSight();
			//Try to turn to face the player
			orientation = (int)Mathf.Sign (GameManager.gameManager.Robot.transform.position.x - transform.position.x);
			Vector3 gunPos = gunPosition.transform.localPosition;
			if (orientation == 1)
				gunPos.x = 2.604553f;
			else
				gunPos.x = -2.604553f;
		
		gunPosition.transform.localPosition = gunPos;
		}
		else
		{
			reaction.OnIntruderOutOfSight();
		}
		
		alertness = reaction.DetermineAlertness();
		if (stunTimer > 0) alertness = Alertness.Normal;
		if (alertness == Alertness.Aggressive)
		{
			if (stunTimer <= 0) reactionMethod.OnAggressive();
			if (previousAlertness != Alertness.Aggressive) 
			{
				if (alertSound != null) AudioSource.PlayClipAtPoint(alertSound, transform.position);

				animations.Play (animations.Library.GetClipByName ("Shoot"));
			}
			activateConnectedGadgets();
			previousAlertness = Alertness.Aggressive;
			
			//Return so we don't move
			return;
		}
		else if (alertness == Alertness.Suspicious)
		{
			reactionMethod.OnSuspicious();
			if (previousAlertness != Alertness.Suspicious)
			{
				if (suspiciousSound != null && previousAlertness != Alertness.Aggressive) 
					AudioSource.PlayClipAtPoint(suspiciousSound, transform.position);
				animations.Play (animations.Library.GetClipByName ("Attack"));
			}
			previousAlertness = Alertness.Suspicious;
			//Don't move if intruder is still in sight
			if (sight.IsObjectInView (GameManager.gameManager.Robot.gameObject)
				|| Vector3.Distance(transform.position, GameManager.gameManager.Robot.transform.position) < bumpRange) 
				return;
		}
		else if (alertness == Alertness.Normal)
		{
			reactionMethod.OnNormal ();
			previousAlertness = Alertness.Normal;
		}
		
		if (stunTimer <= 0)
		{
			movement.UpdateMovement();
			orientation = (int)Mathf.Sign (movement.Velocity.x);
			stunSparks.enableEmission = false;
		}
		Vector3 gunPosi = gunPosition.transform.localPosition;
		if (orientation == 1)
			gunPosi.x = 2.604553f;
		else
			gunPosi.x = -2.604553f;
		
		gunPosition.transform.localPosition = gunPosi;
	}
	
	private void UpdateVisuals()
	{
		//Set our FOV orientation
		sight.Rotation = orientation == 1 ? 0 : 180;
		
		sight.getLight ().LightEnabled = (stunTimer <= 0f);
		
		if (orientation > 0) animations.Sprite.FlipX = true;
		else if (orientation < 0) animations.Sprite.FlipX = false;
		
		if (alertness == Alertness.Normal && animations.CurrentClip.name != "Hover")
		{
			animations.Play (animations.Library.GetClipByName("Hover"));
		}
	}
	
	public void Stun(float seconds)
	{
		stunTimer += seconds;
		stunSparks.enableEmission = true;
	}
}
