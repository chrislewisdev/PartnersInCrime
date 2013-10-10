using UnityEngine;
using System.Collections;

public class Door : AiControllable {
	
	public float closeTime = 1.5f;
	
	// If true, connected gadgets to the door will only be triggered when the ai opens the door, otherwise they will be triggered whenever the door opens
	public bool triggerGadgetsOnlyOnAiOpen = true;
	
	bool open;
	float targetScale;
	GameObject doorCollider;
	BoxCollider boxCollider;
	float closeTimer;
	GameObject controllerButton;
	
	public override void activateGadget(bool triggeredByAi)
	{
		if (triggeredByAi)
		{
			if (open)
			{
				targetScale = 3f;
				boxCollider.enabled = true;
			}
			else
			{
				targetScale = 0f;
				boxCollider.enabled = false;
				activateConnectedGadgets();
			}
			
			open = !open;
		}
		else
		{
			open = true;
			targetScale = 0f;
			boxCollider.enabled = false;
			
			if (!triggerGadgetsOnlyOnAiOpen)
				activateConnectedGadgets();
		}
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	public override void aiLeft ()
	{
		if (controllerButton)
		{
			Destroy(controllerButton);
			controllerButton = null;
		}	
		
		if (open)
		{
			GameObject closeLight = Instantiate(Resources.Load("LightTimer") as GameObject, transform.position, Quaternion.identity) as GameObject;
			closeLight.GetComponent<TimerLight>().revealTime = closeTime;
		}
	}
	
	public override void aiArrived()
	{
		controllerButton = Instantiate(Resources.Load("B Button") as GameObject, transform.position + new Vector3(0f, 3.5f, -1f), Quaternion.identity) as GameObject;
	}
	
	void Start()
	{
		open = false;
		targetScale = 3f;
		doorCollider = transform.GetChild(0).gameObject;
		closeTimer = closeTime;
		boxCollider = GetComponent<BoxCollider>();
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
				if (Vector3.Distance(transform.position, GameManager.gameManager.Robot.gameObject.transform.position) > 3.2f)
				{
					closeTimer = closeTime;
					open = false;
					targetScale = 3f;
					boxCollider.enabled = true;
				}
			}
		}
	}
}
