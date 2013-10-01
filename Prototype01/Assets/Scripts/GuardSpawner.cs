using UnityEngine;
using System.Collections;

public class GuardSpawner : MonoBehaviour {
	
	public GameObject guardPrefab;
	public float number;
	
	static float SPAWN_TIMER = 1f;
	bool _spawning;
	float _spawnTimer;
	int _spawnCounter;
	
	// Spawner will begin spawning guards
	public void spawnGuards()
	{
		_spawning = true;
		_spawnCounter = 0;
		_spawnTimer = SPAWN_TIMER;
	}
	
	void Start()
	{
		_spawning = false;
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
	}
}
