using UnityEngine;
using System.Collections;

public class Door : GadgetControllerInterface {
	
	public float closeTime = 1.5f;
	
	bool open;
	float targetScale;
	GameObject doorCollider;
	BoxCollider collider;
	float closeTimer;
	
	public override void aiSendInput (ButtonState buttonState)
	{
		if (buttonState == ButtonState.BUTTON_DOWN)
		{
			if (open)
			{
				targetScale = 3f;
				collider.enabled = true;
			}
			else
			{
				targetScale = 0f;
				collider.enabled = false;
			}
			
			
			open = !open;
		}
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	void Start()
	{
		open = false;
		targetScale = 3f;
		doorCollider = transform.GetChild(0).gameObject;
		closeTimer = closeTime;
		collider = GetComponent<BoxCollider>();
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
					collider.enabled = true;
					targetScale = 3f;
				}
			}
		}
	}
}
