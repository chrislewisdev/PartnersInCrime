using UnityEngine;
using System.Collections;

public class RetryMenuButtons : MonoBehaviour {

	void Update () {
		if (Input.GetButtonDown("A Button"))
			Application.LoadLevel(Application.loadedLevelName);
		else if (Input.GetButtonDown("Back Button"))
			Application.LoadLevel ("Menu");
	}
}
