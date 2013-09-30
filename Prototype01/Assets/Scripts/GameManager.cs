using UnityEngine;
using System.Collections;

/* Singleton class for managing global variables, functions etc. of game. Will remain
 * loaded throughout multiple scenes */
public class GameManager : MonoBehaviour {
	
	public static GameManager gameManager;
	
	private RobotController robotPlayer;
	public RobotController Robot { get { return robotPlayer; } }
	private AiController aiPlayer;
	public AiController AI { get { return aiPlayer; } }
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		// Check it doesn't already exist
		GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
		if (cameras.Length > 1)
		{
			if (cameras[0] == gameObject)
				cameras[1].GetComponent<GameManager>().updateHandles();
			else
				cameras[0].GetComponent<GameManager>().updateHandles();
			
			Destroy(gameObject);
		}
		else
			updateHandles();
		
		gameManager = this;
	}
	
	// Called when a new level is loaded and updates handles to robot and player objects
	void updateHandles()
	{
		robotPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<RobotController>();
		aiPlayer = GameObject.FindGameObjectWithTag("AI_Player").GetComponent<AiController>();
	}
}