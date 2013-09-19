using UnityEngine;
using System.Collections;

public abstract class ICameraController : MonoBehaviour {

	protected GameObject robotPlayer;
	protected GameObject aiPlayer;
	private tk2dCamera camera;
	
	void Start()
	{
		camera = GetComponent<tk2dCamera>();
		robotPlayer = GameObject.FindGameObjectWithTag("Player");
		aiPlayer = GameObject.FindGameObjectWithTag("AI_Player");
	}
	
	void Update()
	{
		transform.position = getNewPosition();
		camera.ZoomFactor = getNewZoomFactor();
	}
	
	protected abstract Vector3 getNewPosition();
	protected abstract float getNewZoomFactor();
}
