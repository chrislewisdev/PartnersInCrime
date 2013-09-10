using UnityEngine;
using System.Collections;

public class SecurityCamera : GadgetControllerInterface {

	public override void aiSendInput (ButtonState buttonState)
	{
		if (buttonState == ButtonState.BUTTON_DOWN)	
			GetComponent<MeshRenderer>().enabled = false;
		else if (buttonState == ButtonState.BUTTON_UP)
			GetComponent<MeshRenderer>().enabled = true;
		
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, direction.y, 0f));
	}
}
