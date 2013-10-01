using UnityEngine;
using System.Collections;

public enum ButtonState 
{
	BUTTON_DOWN, BUTTON_HOLD, BUTTON_UP, NOT_PRESSED
}

public abstract class GadgetControllerInterface : MonoBehaviour {
	public abstract void aiSendInput(ButtonState buttonState);
	
	public abstract void aiSendDirection(Vector2 direction);
	
	// Returns true if ai player is currently possesing object
	protected bool isPossessed()
	{
		return (GameManager.gameManager.AI.GetComponent<AiController>().occupiedGadget == gameObject);
	}
}
