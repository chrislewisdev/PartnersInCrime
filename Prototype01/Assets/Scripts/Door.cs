﻿using UnityEngine;
using System.Collections;

public class Door : GadgetControllerInterface {
	
	public float closeTime = 1.5f;
	
	bool open;
	Vector3 targetPos;
	GameObject doorCollider;
	GameObject robotPlayer;
	float closeTimer;
	
	public override void aiSendInput (ButtonState buttonState)
	{
		if (buttonState == ButtonState.BUTTON_DOWN)
		{
			if (open)
				targetPos += (new Vector3(0f, -9.6f, 0f));
			else
				targetPos += (new Vector3(0f, 9.6f, 0f));
			
			open = !open;
		}
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
	}
	
	void Start()
	{
		open = false;
		targetPos = transform.position;
		doorCollider = transform.GetChild(0).gameObject;
		closeTimer = closeTime;
		robotPlayer = GameObject.FindGameObjectWithTag("Player"); 
	}
	
	void Update()
	{
		if (doorCollider.transform.position != targetPos)
			doorCollider.transform.position = Vector3.Lerp(doorCollider.transform.position, targetPos, 0.1f);
		
		if (open && !isPossessed())
		{
			closeTimer -= Time.deltaTime;
			if (closeTimer <= 0f)
			{
				if (!robotPlayer.collider.bounds.Contains(transform.position + (new Vector3(-1.5f, -.2f, 0f))) && !robotPlayer.collider.bounds.Contains(transform.position + (new Vector3(1.5f, -.2f, 0f))))
				{
					closeTimer = closeTime;
					open = false;
					targetPos += (new Vector3(0f, -9.6f, 0f));
				}
			}
		}
	}
}
