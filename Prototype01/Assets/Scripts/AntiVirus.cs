﻿using UnityEngine;
using System.Collections;

public class AntiVirus : AiActor {
	
	public float jumpTime;
	public GameObject[] patrolPath;
	
	GameObject aiPlayer;
	
	float timer;
	int pathIndex;
	
	void Start()
	{
		if (patrolPath.Length == 0)
			Debug.Log("Antivirus patrol path is empty");
		else
			jumpToGadget(patrolPath[0]);
		
		timer = jumpTime;
		pathIndex = 1;
		
		aiPlayer = GameObject.FindGameObjectWithTag("AI_Player");
	}
	
	void Update () {
		updateMovement();
		
		if (aiPlayer.GetComponent<AiController>().occupiedGadget == occupiedGadget)
				Debug.Log("CAUGHT BY ANTIVIRUS!!!!!!!!!!!");
		
		timer -= Time.deltaTime;
		if (timer < 0f)
		{
			jumpToGadget(patrolPath[pathIndex++]);
			if (pathIndex == patrolPath.Length)
				pathIndex = 0;
			
			timer = jumpTime;
			
			
		}
	}
}
