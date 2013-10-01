using UnityEngine;
using System.Collections;

// Triggers the alarm when player makes contact with object
public class AlarmTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject == GameManager.gameManager.Robot.gameObject)
		{
			GameManager.gameManager.triggerAlarm();
			Destroy(gameObject);	
		}
	}
}
