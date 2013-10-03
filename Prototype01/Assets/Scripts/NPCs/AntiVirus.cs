using UnityEngine;
using System.Collections;

public class AntiVirus : AiActor {
	
	public float jumpTime;
	public GameObject[] patrolPath;
	
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
	}
	
	void Update () {
		updateMovement();
		
		if (GameManager.gameManager.AI.occupiedGadget == occupiedGadget)
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
