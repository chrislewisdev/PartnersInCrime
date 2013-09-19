using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
public class SecurityCamera : GadgetControllerInterface {
	
	private static float TURN_ON_TIMER = 1.5f;
	
	FieldOfView sight;
	bool sightDeactivated;
	float targetAim;
	float turnOnTimer;
	
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
		if (direction.magnitude > 0.5f)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			targetAim = Mathf.Clamp(angle, 0, 150);
			
			//transform.eulerAngles = new Vector3(0f, 0f, angle);
		}
	}

	void Start()
	{
		sight = GetComponent<FieldOfView>();
		targetAim = transform.eulerAngles.z;
		turnOnTimer = 1.5f;
	}
	
	void Update()
	{
		if (!sightDeactivated)
		{
			if (sight.IsObjectInView(GameObject.FindGameObjectWithTag("Player")))
				Application.LoadLevel (Application.loadedLevelName);
		}
		else if (!isPossessed())
		{
			turnOnTimer -= Time.deltaTime;
			if (turnOnTimer <= 0f)
			{
				sightDeactivated = false;
				sight.enabled = true;
				sight.getLight().LightEnabled = true;
				turnOnTimer = TURN_ON_TIMER;
			}
		}
		
		if (Mathf.Abs(transform.eulerAngles.z - targetAim) > 0.1f)
			transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(transform.eulerAngles.z, targetAim, 0.1f));
	}
}
