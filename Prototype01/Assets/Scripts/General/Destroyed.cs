using UnityEngine;
using System.Collections;

public class Destroyed : MonoBehaviour {

	float timer;
	bool loadedButtonPrefab;
	
	void Start () {
		timer = 4f;
		loadedButtonPrefab = false;
	}
	
	void Update () {
		if (timer > 0f)
			timer -= Time.deltaTime;
		
		if (!loadedButtonPrefab && timer < 1f)
		{
			loadedButtonPrefab = true;
			GameObject buttons = Instantiate(Resources.Load("Menu Retry Buttons") as GameObject, transform.position + new Vector3(0f, -4, 0f), Quaternion.identity) as GameObject;
			buttons.transform.parent = transform;
		}
		
		GetComponent<tk2dSprite>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, 3f - timer));
		transform.position = GameManager.gameManager.transform.position + new Vector3(0f, 0f, 5f);
	}
}
