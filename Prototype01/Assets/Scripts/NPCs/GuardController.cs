using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GuardMovement))]
[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(ReactionLogic))]
[RequireComponent(typeof(ReactionMethod))]
public class GuardController : AiControllable {
	
	private FieldOfView sight;
	private ReactionLogic reaction;
	private ReactionMethod reactionMethod;
	Alertness alertness = Alertness.Normal;
	private bool isActive;
	private int health = 3;
	private GuardMovement movement;
	private tk2dSpriteAnimator animations;
	
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
		sight = GetComponent<FieldOfView>();
		reaction = GetComponent<ReactionLogic>();
		reactionMethod = GetComponent<ReactionMethod>();
		isActive = true;
		movement = GetComponent<GuardMovement>();
		animations = GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isActive) // Do nothing if currently occupied by ai
			return;
		
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
		
		movement.UpdateMovement();
		
		UpdateVisuals();
	}
	
	private void UpdateVisuals()
	{
		//Set our FOV orientation
		sight.Rotation = Mathf.Sign (movement.Velocity.x) == 1 ? 0 : 180;
		
		if (movement.Velocity.x > 0) animations.Sprite.FlipX = true;
		else if (movement.Velocity.x < 0) animations.Sprite.FlipX = false;
	}
}
