using UnityEngine;
using System.Collections;

public class Door : AiControllable {
	
	public float closeTime = 1.5f;
	public AudioClip closeSound;
	public AudioClip openSound;
	
	// If true, connected gadgets to the door will only be triggered when the ai opens the door, otherwise they will be triggered whenever the door opens
	public bool triggerGadgetsOnlyOnAiOpen = true;
	
	bool open;
	float targetScale;
	float beginScale;
	GameObject doorCollider;
	BoxCollider boxCollider;
	float closeTimer;
	GameObject controllerButton;
	bool particleEffectPlayed = false;
	float doorCloseTimer;
	
	public override void activateGadget(bool triggeredByAi)
	{
		if (triggeredByAi)
		{
			if (open)
			{
				boxCollider.enabled = true;
				if (!boxCollider.bounds.Intersects(GameManager.gameManager.Robot.collider.bounds))
				{
					targetScale = 1f;
					beginScale = doorCollider.transform.localScale.y;
					doorCloseTimer = 0f;
					if (closeSound != null) AudioSource.PlayClipAtPoint(closeSound, transform.position);
					open = false;
					particleEffectPlayed = false;
				}
				else
					boxCollider.enabled = false;
			}
			else
			{
				targetScale = 0f;
				doorCloseTimer = 0f;
				beginScale = doorCollider.transform.localScale.y;
				boxCollider.enabled = false;
				if (openSound != null) AudioSource.PlayClipAtPoint(openSound, transform.position);
				activateConnectedGadgets();
				open = true;
			}
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
		controllerButton = Instantiate(Resources.Load("X Button") as GameObject, transform.position + new Vector3(0f, 3.5f, -1f), Quaternion.identity) as GameObject;
	}
	
	void Start()
	{
		open = false;
		targetScale = 3f;
		beginScale = 0f;
		doorCloseTimer = 1.0f;
		doorCollider = transform.GetChild(0).gameObject;
		closeTimer = closeTime;
		boxCollider = GetComponent<BoxCollider>();
		particleEffectPlayed = true;
	}
	
	void Update()
	{
		if (doorCloseTimer < 1.0f)
		{
			doorCloseTimer += Time.deltaTime * 3f;
			if (doorCloseTimer > 1.0f)
				doorCloseTimer = 1.0f;
			
			float y = Mathfx.Coserp(beginScale, targetScale, doorCloseTimer);
			doorCollider.transform.localScale = new Vector3(1f, y, 1f);
			
			if (!particleEffectPlayed && doorCollider.transform.localScale.y >= 0.9f)
			{
				Instantiate(Resources.Load("FloorHit") as GameObject, transform.position + new Vector3(0f, -collider.bounds.extents.y - 0.5f, -1f), Quaternion.identity);
				particleEffectPlayed = true;
			}
		}
		
		if (open && !isPossessed())
		{
			closeTimer -= Time.deltaTime;
			if (closeTimer <= 0f)
			{
				if (Vector3.Distance(transform.position, GameManager.gameManager.Robot.gameObject.transform.position) > 5.2f)
				{
					closeTimer = closeTime;
					open = false;
					targetScale = 1f;
					beginScale = doorCollider.transform.localScale.y;
					doorCloseTimer = 0f;
					boxCollider.enabled = true;
					if (closeSound != null) AudioSource.PlayClipAtPoint(closeSound, transform.position);
					particleEffectPlayed = false;
				}
			}
		}
	}
}
