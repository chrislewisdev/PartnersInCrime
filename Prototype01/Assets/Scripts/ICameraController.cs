using UnityEngine;
using System.Collections;

public abstract class ICameraController : MonoBehaviour {

	protected GameObject robotPlayer;
	protected GameObject aiPlayer;
	protected GameObject selectionCircle;
	protected tk2dCamera tkCamera;
	private AiController aiController;
	
	void Start()
	{
		tkCamera = GetComponent<tk2dCamera>();
		robotPlayer = GameObject.FindGameObjectWithTag("Player");
		aiPlayer = GameObject.FindGameObjectWithTag("AI_Player");
		aiController = aiPlayer.GetComponent<AiController>();
	}
	
	void Update()
	{
		selectionCircle = aiController.getSelectionCircle();
		transform.position = getNewPosition();
		tkCamera.ZoomFactor = Mathf.Lerp(tkCamera.ZoomFactor, getNewZoomFactor(), 0.05f);
	}
	
	protected abstract Vector3 getNewPosition();
	protected abstract float getNewZoomFactor();
}
