using UnityEngine;
using System.Collections;

// Base class to use when implementing antivirus ai
public class AiActor : MonoBehaviour {

	public GameObject occupiedGadget;
	
	public void jumpToGadget(GameObject gadget)
	{
		transform.parent = gadget.transform;
		transform.localPosition = new Vector3(0f, 0f, -.1f);

		occupiedGadget = gadget;
	}
	
	public void sendGadgetButtonInput(ButtonState buttonState)
	{
		occupiedGadget.GetComponent<GadgetControllerInterface>().aiSendInput(buttonState);
	}
	
	public void sendGadgetDirectionInput(Vector2 direction)
	{
		occupiedGadget.GetComponent<GadgetControllerInterface>().aiSendDirection(direction);
	}
	
	void Start()
	{
		;
	}
}
