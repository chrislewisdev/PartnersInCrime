using UnityEngine;
using System.Collections;

public class Destroyed : MonoBehaviour {

	float timer;
	
	void Start () {
		timer = 4f;
	}
	
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0f)
		{
			Application.LoadLevel ("Menu");
		}
		
		GetComponent<tk2dSprite>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, 3f - timer));
		transform.position = GameManager.gameManager.transform.position + new Vector3(0f, 0f, 5f);
	}
}
