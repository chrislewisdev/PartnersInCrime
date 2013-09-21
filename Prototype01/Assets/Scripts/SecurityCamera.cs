using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
public class SecurityCamera : GadgetControllerInterface {
	
	public float startRotation;
	public float endRotation;
	public float rotationSpeed;
	
	private static float TURN_ON_TIMER = 1.5f;
	
	FieldOfView sight;
	bool sightDeactivated;
	float turnOnTimer;
	bool panningRight;
	
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

		transform.eulerAngles = new Vector3(0f, 0f, startRotation);
		turnOnTimer = 1.5f;
        panningRight = true;
	}
	
	void Update()
	{
		if (!sightDeactivated)
		{
			if (sight.IsObjectInView(GameObject.FindGameObjectWithTag("Player")))
				Application.LoadLevel (Application.loadedLevelName);

			updateRotation();
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
		sight.setRotation(angle);
	}
}
