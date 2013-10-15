using UnityEngine;
using System.Collections;

public class EscapeTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if (collider.collider.gameObject == GameManager.gameManager.Robot.gameObject && GameManager.gameManager.alarmTriggered)
			Debug.Log("YOU WIIIIIIIIIIIIIINNN!!!!!!!!!!!!!!!!");
	}
}
