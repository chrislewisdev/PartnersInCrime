using UnityEngine;
using System.Collections;

// Spawns a turret when triggered or alarm goes off
public class TurretSpawner : Spawner {
	// If false, will only spawn when triggered by another object
	public bool spawnOnAlarm = true;
	private GameObject turret;
	
	public override void spawn ()
	{
		turret.SetActive(true);
	}
	
	void Start()
	{
		if (transform.childCount > 0)
		{
			turret = transform.GetChild(0).gameObject;
			turret.SetActive(false);
		}
		else
		{
			Debug.LogError("Turretspawner does not have a turret child to spawn, add a turret as a child of the turretSpawner");
		}
		
		GameManager.gameManager.registerSpawner(this);
	}
}
