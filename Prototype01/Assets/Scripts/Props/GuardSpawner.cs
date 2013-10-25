using UnityEngine;
using System.Collections;

public class GuardSpawner : Spawner {
	
	public GameObject guardPrefab;
	public float number;
	public bool spawnOnce;
	
	static float SPAWN_TIMER = 1f;
	bool _spawning;
	bool _guardsSpawned;
	float _spawnTimer;
	int _spawnCounter;
	bool _triggeredLastStep;
	bool _triggeredThisStep;
	
	// Spawner will begin spawning guards
	public override void spawn ()
	{
		_triggeredThisStep = true;
		
		if ((spawnOnce && _guardsSpawned) || _triggeredLastStep)
			return;
		
		if (!_spawning)
		{
			_spawning = true;
			_guardsSpawned = true;
			_spawnCounter = 0;
			_spawnTimer = SPAWN_TIMER;
		}
	}
	
	void Start()
	{
		_spawning = false;
		_guardsSpawned = false;
		GameManager.gameManager.registerSpawner(this);
	}
	
	void Update()
	{	
		if (_spawning)
		{
			_spawnTimer += Time.deltaTime;
			if (_spawnTimer > SPAWN_TIMER)
			{
				_spawnTimer = 0f;
				_spawnCounter++;
				Instantiate(guardPrefab, new Vector3(transform.position.x, transform.position.y, 10f), Quaternion.identity);
				
				if (_spawnCounter >= number)
					_spawning = false;
			}
		}
		
		if (_triggeredThisStep)
		{
			_triggeredLastStep = true;
			_triggeredThisStep = false;
		}
		else
			_triggeredLastStep = false;
	}
}
