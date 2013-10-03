using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
public class SecurityCamera : GadgetControllerInterface {
	
	public float startRotation;
	public float endRotation;
	public float rotationSpeed;
	
	public float turnOnTimer = 1.5f;
	
	private FieldOfView sight;
	private bool sightDeactivated;
	private float turnOnTimerCount;
	private bool panningRight;
	private tk2dSprite sprite;
	private ReactionLogic reaction;
	
	public override void aiSendInput (ButtonState buttonState)
	{	
		if (buttonState == ButtonState.BUTTON_DOWN)
		{
			if (!sightDeactivated)
			{
				sightDeactivated = true;
				sight.getLight().LightEnabled = false;
				sight.enabled = false;
			
			}
			else
			{
				sightDeactivated = false;
				sight.enabled = true;
				sight.getLight().LightEnabled = true;
			}
		}
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
	}

	void Start()
	{
		sight = GetComponent<FieldOfView>();
		reaction = GetComponent<ReactionLogic>();
		sprite = GetComponent<tk2dSprite>();

		transform.eulerAngles = new Vector3(0f, 0f, startRotation);
		turnOnTimerCount = turnOnTimer; 
        panningRight = true;
	}
	
	void Update()
	{
		if (!sightDeactivated)
		{
			if (sight.IsObjectInView(GameObject.FindGameObjectWithTag("Player")))
				reaction.OnIntruderInSight ();
			else
				reaction.OnIntruderOutOfSight();
			
			Alertness alertness = reaction.DetermineAlertness();
			if (alertness == Alertness.Aggressive)
			{
				//Application.LoadLevel (Application.loadedLevelName);
				sight.getLight ().LightColor = Color.red;
			}
			else if (alertness == Alertness.Suspicious)
			{
				sight.getLight ().LightColor = Color.yellow;
			}
			else if (alertness == Alertness.Normal)
			{
				sight.getLight().LightColor = Color.blue;
			}

			updateRotation();
		}
		else if (!isPossessed())
		{
			turnOnTimerCount -= Time.deltaTime;
			if (turnOnTimerCount <= 0f)
			{
				sightDeactivated = false;
				sight.enabled = true;
				sight.getLight().LightEnabled = true;
				turnOnTimerCount = turnOnTimer;
			}
		}
	}
	
	void updateRotation()
	{
		float angle = transform.localEulerAngles.z;
		
		if (panningRight)
		{
			if (angle >= endRotation)
				panningRight = false;
			else
				angle += Time.deltaTime * rotationSpeed;
		}
		else
		{
			if (angle <= startRotation)
				panningRight = true;
			else
				angle -= Time.deltaTime * rotationSpeed;
		}
		
		transform.eulerAngles = new Vector3(0f, 0f, angle);
		if (angle > 90f && angle < 270f)
			sprite.SetSprite(3);
		else
			sprite.SetSprite(2);
		
		sight.setRotation(angle);
	}
}
