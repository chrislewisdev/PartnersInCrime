using UnityEngine;
using System.Collections;

// Base class to use when implementing antivirus ai
public class AiActor : MonoBehaviour {

	public GameObject occupiedGadget;
	
	private float moveTimer = 0f;
	private bool moving = false;
	private Vector3 beginPosition;
	
	//Moves ai player to new gadget
	protected void jumpToGadget(GameObject gadget)
	{
		/*transform.parent = gadget.transform;
		transform.localPosition = new Vector3(0f, 0f, -.1f);
		 */
		occupiedGadget = gadget;
		moving = true;
		moveTimer = 0f;
		beginPosition = transform.position;
		transform.parent = null;
	}
	
	//Sends button input to currently occupied gadget
	protected void sendGadgetButtonInput(ButtonState buttonState)
	{
		occupiedGadget.GetComponent<GadgetControllerInterface>().aiSendInput(buttonState);
	}
	
	//Sends direction input to currently occupied gadget
	protected void sendGadgetDirectionInput(Vector2 direction)
	{
		occupiedGadget.GetComponent<GadgetControllerInterface>().aiSendDirection(direction);
	}
	
	//Updates movement of ai actor
	protected void updateMovement()
	{		
		if (moving)
		{
			moveTimer += Time.deltaTime * 10f;
			if (moveTimer > 1.0f)
			{
				moveTimer = 1f;
				moving = false;
				transform.parent = occupiedGadget.transform;
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				transform.localRotation = Quaternion.identity;
			}
			else
				transform.position = Vector3.Lerp(beginPosition, occupiedGadget.transform.position, moveTimer);
		}
		
	}
}
