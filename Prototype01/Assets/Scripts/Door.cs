using UnityEngine;
using System.Collections;

public class Door : GadgetControllerInterface {
	
	bool open;
	Vector3 targetPos;
	GameObject doorCollider;
	
	public override void aiSendInput (ButtonState buttonState)
	{
		if (buttonState == ButtonState.BUTTON_DOWN)
		{
			if (open)
				targetPos += (new Vector3(0f, -3.2f, 0f));
			else
				targetPos += (new Vector3(0f, 3.2f, 0f));
			
			open = !open;
		}
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	void Start()
	{
		open = false;
		targetPos = transform.position;
		doorCollider = transform.GetChild(0).gameObject;
	}
	
	void Update()
	{
		if (doorCollider.transform.position != targetPos)
			doorCollider.transform.position = Vector3.Lerp(doorCollider.transform.position, targetPos, 0.1f);
	}
}
