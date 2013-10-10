using UnityEngine;
using System.Collections;

// Base class to use when implementing antivirus ai
public class AiActor : MonoBehaviour {

	public GameObject occupiedGadget;
	
	private float moveTimer = 0f;
	private bool moving = false;
	private Vector3 beginPosition;
	private ParticleSystem particles;
	
	//Called when occupied gadget is destroyed
	public void occupiedGadgetDestroyed()
	{
		jumpToGadget(GameManager.gameManager.Robot.gameObject);
	}
	
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
		if (particles)
			particles.enableEmission = true;
	}
	
	//Sends button input to currently occupied gadget
	protected void sendGadgetButtonInput(ButtonState buttonState)
	{
		occupiedGadget.GetComponent<AiControllable>().aiSendInput(buttonState);
	}
	
	//Sends direction input to currently occupied gadget
	protected void sendGadgetDirectionInput(Vector2 direction)
	{
		occupiedGadget.GetComponent<AiControllable>().aiSendDirection(direction);
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
				if (particles)
					particles.enableEmission = false;
			}
			else
				transform.position = Vector3.Lerp(beginPosition, occupiedGadget.transform.position, moveTimer);
		}
		else
			transform.position = occupiedGadget.transform.position;
	}
	
	//Initilizes ai actor and partile systems
	protected void initilize()
	{
		particles = null;
		
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.name == "Particle System")
			{
				particles = transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
				break;
			}
		}
		
		if (particles)
			particles.enableEmission = false;
	}
}