using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : AiControllable
{
	public float riseNumberOfBlocks;
	public float fallNumberOfBlocks;
	public float rightNumberOfBlocks;
	public float leftNumberOfBlocks;
	
	private Vector3 lastMovement = Vector3.zero;
	private float maxHeight;
	private float minHeight;
	private float leftMax;
	private float rightMax;
	private GameObject arrows;
	private GameObject arrowsPrefab;
	
	public override void activateGadget (bool triggeredByAi)
	{
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
		lastMovement = new Vector3(direction.x, direction.y, 0f) / 10f;
		Vector3 newPos = transform.position + lastMovement;
			
		if (newPos.y > maxHeight)
			lastMovement.y -= newPos.y - maxHeight;
		else if (newPos.y < minHeight)
			lastMovement.y += minHeight - newPos.y;
		
		if (newPos.x > rightMax)
			lastMovement.x -= newPos.x - rightMax;
		else if (newPos.x < leftMax)
			lastMovement.x += leftMax - newPos.x;
		
		transform.position += lastMovement;
		
		if (lastMovement.magnitude > 0.02f)
			activateConnectedGadgets();
	}
	
	public override void aiArrived ()
	{
		arrows = Instantiate(arrowsPrefab) as GameObject;
		arrows.transform.parent = transform;
		arrows.transform.localPosition = new Vector3(1f, 0f, -1f);
	}
	
	public override void aiLeft ()
	{
		if (arrows)
		{
			Destroy(arrows);
			arrows = null;
		}
	}
		
	void Start()
	{
		maxHeight = transform.position.y + 3.2f * riseNumberOfBlocks;
		minHeight = transform.position.y - 3.2f * fallNumberOfBlocks;
		leftMax = transform.position.x - 3.2f * leftNumberOfBlocks;
		rightMax = transform.position.x + 3.2f * rightNumberOfBlocks;
		arrows = null;
		if (riseNumberOfBlocks != 0 || fallNumberOfBlocks != 0)
		{
			if (leftNumberOfBlocks == 0 && rightNumberOfBlocks == 0)
				arrowsPrefab = Resources.Load("UpDownArrows") as GameObject;
			else
				arrowsPrefab = Resources.Load("AllDirectionArrows") as GameObject;
		}
		else if (leftNumberOfBlocks != 0 || rightNumberOfBlocks != 0)
			arrowsPrefab = Resources.Load("LeftRightArrows") as GameObject;
		else
			Debug.LogError("Moving platform has no movement set");
	}
	
	void LateUpdate()
	{
		GameObject robot = GameManager.gameManager.Robot.gameObject;
		RobotMovement movement = robot.GetComponent<RobotMovement>();
		
		if (collider.bounds.Intersects(robot.collider.bounds))
		{
			if (robot.transform.position.y > transform.position.y)
			{
				float penetrationAmount = Mathf.Abs(robot.transform.position.y - transform.position.y) - (robot.collider.bounds.extents.y + collider.bounds.extents.y) - .01f;
				robot.transform.Translate(new Vector3(0f, -penetrationAmount, 0f));
				movement.OnPlatform = true;	
			}
			else
				movement.OnPlatform = false;
		}
		else
			movement.OnPlatform = false;
	}
}
