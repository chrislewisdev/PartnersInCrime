using UnityEngine;
using System.Collections;

/// <summary>
/// StartScreen is a simple script that will load a target level when the Start button is pressed.
/// </summary>
public class StartScreen : MonoBehaviour {
	
	public string targetLevel;
	public tk2dSprite menuImage;
	bool showingCredits;
	
	void Start()
	{
		showingCredits = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool start = (Input.GetButtonDown ("Start Button"));
		bool back = (Input.GetButtonDown("Back Button"));
		bool AButton = (Input.GetButtonDown("A Button"));
		
		if (AButton && !showingCredits) 
			Application.LoadLevel (targetLevel);
		else if (back && !showingCredits)
			Application.Quit();
		else if (back && showingCredits)
		{
			showingCredits = false;
			menuImage.SetSprite("home");	
		}
		else if (start && !showingCredits)
		{
			showingCredits = true;
			menuImage.SetSprite("credits");
		}
	}
}
