using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if (collider.collider.gameObject == GameManager.gameManager.Robot.gameObject)
		{
			Instantiate(Resources.Load("Success") as GameObject);
			Destroy(gameObject);
		}
	}
}
