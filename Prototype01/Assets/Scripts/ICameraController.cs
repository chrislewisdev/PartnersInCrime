using UnityEngine;
using System.Collections;

public abstract class ICameraController : MonoBehaviour {

	public GameObject robotPlayer;
	public GameObject aiPlayer;
	private tk2dCamera camera;
	
	void Start()
	{
		camera = GetComponent<tk2dCamera>();
	}
	
	void Update()
	{
		transform.position = getNewPosition();
		camera.ZoomFactor = getNewZoomFactor();
	}
	
	protected abstract Vector3 getNewPosition();
	protected abstract float getNewZoomFactor();
}
