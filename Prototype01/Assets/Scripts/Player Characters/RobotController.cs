using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RobotMovement))]
public class RobotController : GadgetControllerInterface {
	
	private tk2dSpriteAnimator animations;
	private float timeSinceLastHit = 0f;
	private RobotMovement movement;
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	public override void aiSendInput (ButtonState buttonState)
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
		if (movement.Velocity.y > 0)
		{
			animations.Play (animations.Library.GetClipByName ("Jump_Start"));
		}
		else if (movement.Velocity.y < 0)
		{
			animations.Play (animations.Library.GetClipByName ("Jump_End"));
		}
	}
}
