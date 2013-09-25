using UnityEngine;
using System.Collections;

public class CameraControllerTwoPoint : ICameraController {

	protected override Vector3 getNewPosition()
	{
		Vector3 averagePos = robotPlayer.transform.position + aiPlayer.transform.position;
		/*if (selectionCircle)
		{
			averagePos += selectionCircle.transform.position;
			averagePos /= 3.0f;
		}
		else*/
			averagePos /= 2.0f;
		
		averagePos.z = -10f;
		return Vector3.Lerp(transform.position, averagePos, 0.1f);
	}
	
	protected override float getNewZoomFactor()
	{
		Vector3 robotPoint = tkCamera.camera.WorldToViewportPoint(robotPlayer.transform.position);
		Vector3 aiPoint = tkCamera.camera.WorldToViewportPoint(aiPlayer.transform.position);
		
		return 1.0f - Vector3.Distance(robotPoint, aiPoint) / 3.0f;
	}
}
