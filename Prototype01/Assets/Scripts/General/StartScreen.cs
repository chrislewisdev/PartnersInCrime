using UnityEngine;
using System.Collections;

/// <summary>
/// StartScreen is a simple script that will load a target level when the Start button is pressed.
/// </summary>
public class StartScreen : MonoBehaviour {
	
	public string targetLevel;
	
	// Update is called once per frame
	void Update () 
	{
		float start = Input.GetAxis ("Start");
		
		if (start != 0f) Application.LoadLevel (targetLevel);
	}
}
