using UnityEngine;
using System.Collections;

public abstract class ICameraController : MonoBehaviour {

	protected tk2dCamera tkCamera;
	
	void Start()
	{
		tkCamera = GetComponent<tk2dCamera>();
	}
	
	void Update()
	{
		transform.position = getNewPosition();
		tkCamera.ZoomFactor = Mathf.Lerp(tkCamera.ZoomFactor, getNewZoomFactor(), 0.05f);
	}
	
	protected abstract Vector3 getNewPosition();
	protected abstract float getNewZoomFactor();
}
