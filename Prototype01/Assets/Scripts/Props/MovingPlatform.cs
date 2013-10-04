using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : GadgetControllerInterface
{
	public float riseNumberOfBlocks;
	public float fallNumberOfBlocks;
	
	private List<CharacterController> carryingObjects = new List<CharacterController>();
	private Vector3 lastMovement = Vector3.zero;
	private bool updatingPositions = false;
	private float maxHeight;
	private float minHeight;
	
	public override void aiSendInput (ButtonState buttonState)
	{
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
		lastMovement = new Vector3(0f, direction.y, 0f) / 10f;
		
		float newY = (transform.position + lastMovement).y;
		if (newY > maxHeight)
			lastMovement.y -= newY - maxHeight;
		else if (newY < minHeight)
			lastMovement.y += minHeight - newY;
		
		transform.position += lastMovement;
	}
	
	public void characterCollision(GameObject character)
	{
		if (!updatingPositions)
		{
			carryingObjects.Add(character.GetComponent<CharacterController>());	
		}
	}
		
	void Start()
	{
		maxHeight = transform.position.y + 3.2f * riseNumberOfBlocks;
		minHeight = transform.position.y - 3.2f * fallNumberOfBlocks;
	}
	
	void LateUpdate()
	{
		updatingPositions = true;
		foreach (CharacterController o in carryingObjects)
		{
			o.Move(lastMovement);	
		}
		
		carryingObjects.Clear();
		updatingPositions = false;
	}
}
