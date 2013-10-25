using UnityEngine;
using System.Collections.Generic;

/* Singleton class for managing global variables, functions etc. of game. Will remain
 * loaded throughout multiple scenes */
public class GameManager : MonoBehaviour {
	
	public static GameManager gameManager;
	
	private RobotController robotPlayer;
	public RobotController Robot { get { return robotPlayer; } }
	private AiController aiPlayer;
	public AiController AI { get { return aiPlayer; } }
	public tk2dTileMap ladderMap;
	
	private List<Spawner> spawners = new List<Spawner>();
	public bool alarmTriggered;
	
	// Triggers alarm, including triggering all registered guard spawners
	public void triggerAlarm()
	{
		if (!alarmTriggered)
		{
			foreach (Spawner spawner in spawners)
				spawner.spawn();
			
			
			robotPlayer.powerUpRobot();
			alarmTriggered = true;
		}
		
		Debug.Log("Alarm triggered");
	}
	
	// Registers a guard spawner that will be triggered when the alarm goes off
	public void registerSpawner(Spawner spawner)
	{
		spawners.Add(spawner);
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
		{
			updateHandles();
			gameManager = this;
		}
	}
	
	void OnLevelWasLoaded(int level)
	{
		updateHandles ();
	}
	
	// Called when a new level is loaded and updates handles to robot and player objects
	void updateHandles()
	{
		robotPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<RobotController>();
		aiPlayer = GameObject.FindGameObjectWithTag("AI_Player").GetComponent<AiController>();
		alarmTriggered = false;
	}
	
	/*void OnDestroy()
	{
		gameManager = null;
	}*/
}