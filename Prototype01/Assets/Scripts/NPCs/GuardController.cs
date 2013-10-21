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
			Destroy(gameObject);
		}
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isActive) // Do nothing if currently occupied by ai
			return;
		
		UpdateVisuals();
		
		if (sight.IsObjectInView (GameManager.gameManager.Robot.gameObject)
			|| Vector3.Distance(transform.position, GameManager.gameManager.Robot.transform.position) < 7f)
		{
			reaction.OnIntruderInSight();
			//Try to turn to face the player
			orientation = (int)Mathf.Sign (GameManager.gameManager.Robot.transform.position.x - transform.position.x);
		}
		else
		{
			reaction.OnIntruderOutOfSight();
		}
		
		alertness = reaction.DetermineAlertness();
		if (alertness == Alertness.Aggressive)
		{
			reactionMethod.OnAggressive();
			if (previousAlertness != Alertness.Aggressive && alertSound != null) 
			{
				AudioSource.PlayClipAtPoint(alertSound, transform.position);
			}
			activateConnectedGadgets();
			previousAlertness = Alertness.Aggressive;
			//Return so we don't move
			return;
		}
		else if (alertness == Alertness.Suspicious)
		{
			reactionMethod.OnSuspicious();
			if (previousAlertness != Alertness.Suspicious && previousAlertness != Alertness.Aggressive && suspiciousSound != null) 
				AudioSource.PlayClipAtPoint(suspiciousSound, transform.position);
			previousAlertness = Alertness.Suspicious;
			//Don't move if intruder is still in sight
			if (sight.IsObjectInView (GameManager.gameManager.Robot.gameObject)
				|| Vector3.Distance(transform.position, GameManager.gameManager.Robot.transform.position) < 7f) 
				return;
		}
		else if (alertness == Alertness.Normal)
		{
			reactionMethod.OnNormal ();
			previousAlertness = Alertness.Normal;
		}
		
		movement.UpdateMovement();
		orientation = (int)Mathf.Sign (movement.Velocity.x);
	}
	
	private void UpdateVisuals()
	{
		//Set our FOV orientation
		sight.Rotation = orientation == 1 ? 0 : 180;
		
		if (orientation > 0) animations.Sprite.FlipX = true;
		else if (orientation < 0) animations.Sprite.FlipX = false;
	}
}
