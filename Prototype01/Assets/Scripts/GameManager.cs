using UnityEngine;
using System.Collections;

/* Singleton class for managing global variables, functions etc. of game. Will remain
 * loaded throughout multiple scenes */
public class GameManager : MonoBehaviour {
	
	public static GameManager gameManager;
	
	GameObject robotPlayer;
	GameObject aiPlayer;
	
	public GameObject getRobotPlayer()
	{
		return robotPlayer;
	}
	
	public GameObject getAiPlayer()
	{
		return aiPlayer;
	}
	
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
		robotPlayer = GameObject.FindGameObjectWithTag("Player");
		aiPlayer = GameObject.FindGameObjectWithTag("AI_Player");
	}
}