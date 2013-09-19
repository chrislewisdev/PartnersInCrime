using UnityEngine;
using System.Collections;

public class CameraControllerTwoPoint : ICameraController {

	protected override Vector3 getNewPosition()
	{
		Vector3 averagePos = robotPlayer.transform.position + aiPlayer.transform.position;
		averagePos /= 2.0f;
		
		averagePos.z = -10f;
		return Vector3.Lerp(transform.position, averagePos, 0.1f);
	}
	
	protected override float getNewZoomFactor()
	{
		return 1.0f;
	}
}
