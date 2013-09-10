using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {
	
	//Editor variables
	public float fieldOfView;
	public float range;
	
	private float rotation = 0f;
	public float Rotation { get { return rotation; } set { rotation = value; } }
	
	public void Update()
	{
		/*Vector2 mouse = GameObject.Find ("AngleObject").transform.position;
		Vector2 position = transform.position;
		
		Vector3 d = mouse - position;
		Debug.Log (Mathf.Atan2 (d.y, d.x) * Mathf.Rad2Deg);*/
	}
	
	//Determines whether the specified point is in view of this object.
	public bool IsPointInView(Vector3 target)
	{
		float distance = Vector3.Distance (target, transform.position);
		if (distance > range) return false;
		
		//Calculate the angle between our viewpoint and the target
		//Make sure we're Z-aligned so the angle is purely in 2D space
		float angle = AngleToPoint (target);
		
		return false;
	}
	
	//Returns the angle in 2D to a specific target
	public float AngleToPoint(Vector2 target)
	{
		Vector2 position = transform.position;
		Vector2 d = target - position;
		return Mathf.Atan2 (d.y, d.x) * Mathf.Rad2Deg;
	}
}
