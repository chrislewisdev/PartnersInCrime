using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class RobotAiDistanceIndicator : MonoBehaviour {
	private LineRenderer lineRenderer;
	private RobotController robot;
	private AiController ai;
	
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
		lineRenderer.SetVertexCount(2);
		
		robot = GameManager.gameManager.Robot;
		ai = GameManager.gameManager.AI;
	}
	
	void Update () {
		float distance = Vector3.Distance(robot.transform.position, ai.transform.position);
		if (distance > ai.maxRobotDistance / 2f)
		{
			float min = ai.maxRobotDistance / 2f;
			float max = ai.maxRobotDistance;
			float val =(distance - ai.maxRobotDistance / 2f) / (ai.maxRobotDistance / 2f);
			
			lineRenderer.enabled = true;
			lineRenderer.SetPosition(0, ai.transform.position);
			lineRenderer.SetPosition(1, robot.transform.position);
			lineRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, val);
		}
		else
			lineRenderer.enabled = false;
	}
}
