using UnityEngine;
using System.Collections.Generic;

public enum ButtonState 
{
	BUTTON_DOWN, BUTTON_HOLD, BUTTON_UP, NOT_PRESSED
}

//Defines if an object can be controlled by the AI, as well as be activated and activate other objects
public abstract class AiControllable : GadgetInterface {
	
	public void aiSendInput(ButtonState buttonState)
	{
		if (buttonState == ButtonState.BUTTON_DOWN)
		{
			activateGadget(true);
		}
	}
		
	public abstract void aiSendDirection(Vector2 direction);
	
	// Called when the ai player possesses this object
	public abstract void aiArrived();
	
	// Called when ai player leaves this object
	public abstract void aiLeft();
	
	// Returns true if ai player is currently possesing object
	protected bool isPossessed()
	{
		return (GameManager.gameManager.AI.GetComponent<AiController>().occupiedGadget == gameObject);
	}
}
