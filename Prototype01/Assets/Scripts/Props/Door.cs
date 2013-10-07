using UnityEngine;
using System.Collections;

public class Door : GadgetControllerInterface {
	
	public float closeTime = 1.5f;
	
	bool open;
	float targetScale;
	GameObject doorCollider;
	BoxCollider boxCollider;
	float closeTimer;
	GameObject controllerButton;
	
	public override void aiSendInput (ButtonState buttonState)
	{
		if (buttonState == ButtonState.BUTTON_DOWN)
		{
			if (open)
			{
				targetScale = 3f;
				boxCollider.enabled = true;
			}
			else
			{
				targetScale = 0f;
				boxCollider.enabled = false;
			}
			
			
			open = !open;
		}
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	public override void aiLeft ()
	{
		if (controllerButton)
		{
			Destroy(controllerButton);
			controllerButton = null;
		}	
		
		if (open)
		{
			GameObject lightTimer = Instantiate(Resources.Load("LightTimer") as GameObject, transform.position, Quaternion.identity) as GameObject;
			lightTimer.GetComponent<TimerLight>().revealTime = closeTime;
		}
	}
	
	public override void aiArrived()
	{
		controllerButton = Instantiate(Resources.Load("B Button") as GameObject, transform.position + new Vector3(0f, 3.5f, -1f), Quaternion.identity) as GameObject;
	}
	
	void Start()
	{
		open = false;
		targetScale = 3f;
		doorCollider = transform.GetChild(0).gameObject;
		closeTimer = closeTime;
		boxCollider = GetComponent<BoxCollider>();
	}
	
	void Update()
	{
		if (doorCollider.transform.localScale.z != targetScale)
		{
			float y = Mathf.Lerp(doorCollider.transform.localScale.y, targetScale, 0.1f);
			doorCollider.transform.localScale = new Vector3(1f, y, 1f);
		}
		
		if (open && !isPossessed())
		{
			closeTimer -= Time.deltaTime;
			if (closeTimer <= 0f)
			{
				if (!GameManager.gameManager.Robot.gameObject.collider.bounds.Contains(transform.position + 
					(new Vector3(-1.5f, -.2f, 0f))) && !GameManager.gameManager.Robot.gameObject.collider.bounds.Contains(transform.position + (new Vector3(1.5f, -.2f, 0f))))
				{
					closeTimer = closeTime;
					open = false;
					boxCollider.enabled = true;
					targetScale = 3f;
				}
			}
		}
	}
}
