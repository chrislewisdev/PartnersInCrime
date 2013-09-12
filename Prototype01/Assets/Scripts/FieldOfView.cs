using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {
	
	//Editor variables
	public float fieldOfView;
	public float range;
	public Material material;
	
	private float rotation = 0f;
	public float Rotation { get { return rotation; } set { rotation = value; } }
	
	public void Update()
	{
		/*Vector2 mouse = GameObject.Find ("AngleObject").transform.position;
		Vector2 position = transform.position;
		
		Vector3 d = mouse - position;
		Debug.Log (Mathf.Atan2 (d.y, d.x) * Mathf.Rad2Deg);*/
		//Debug.DrawRay (transform.position, new Vector3(Mathf.Cos (rotation * Mathf.Deg2Rad), Mathf.Sin (rotation * Mathf.Deg2Rad)));
		//Debug.Log (IsObjectInView (GameObject.Find ("AngleObject")));
	}
	
	void OnPostRender()
	{
		float angle1 = rotation - fieldOfView/2, angle2 = rotation + fieldOfView/2;
		Vector3 p1 = new Vector3(Mathf.Cos (angle1 * Mathf.Deg2Rad) * range, Mathf.Sin (angle1 * Mathf.Deg2Rad) * range, transform.position.z);
		Vector3 p2 = new Vector3(Mathf.Cos (angle2 * Mathf.Deg2Rad) * range, Mathf.Sin (angle2 * Mathf.Deg2Rad) * range, transform.position.z);
		GL.PushMatrix();
		material.SetPass (0);
		GL.Color (Color.yellow);
		//GL.modelview = camera.projectionMatrix;
		//GL.LoadProjectionMatrix(camera.projectionMatrix);
		//GL.LoadOrtho ();
		GL.Begin (GL.LINES);
			GL.Vertex(transform.position);
			GL.Vertex (p1);
			//GL.Vertex (p2);
			//GL.Vertex3 (0, 0, 0);
			//GL.Vertex3 (1, 1, 0);
			//GL.Vertex3 (10, 20, 0);
		GL.End ();
		GL.PopMatrix();
	}
	
	//Determines whether the specified object is in view of this object.
	public bool IsObjectInView(GameObject target)
	{
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if (distance > range) return false;
		
		//Calculate the angle between our viewpoint and the target
		//Make sure we're Z-aligned so the angle is purely in 2D space
		float deltaAngle = Mathf.DeltaAngle (rotation, AngleToPoint (target.transform.position));
		if (deltaAngle > fieldOfView / 2) return false;
		
		//If target is both in range and in view, check that line of sight is not blocked
		Ray lineOfSight = new Ray(transform.position, target.transform.position - transform.position);
		RaycastHit hitInfo;
		if (Physics.Raycast (lineOfSight, out hitInfo, range, LayerMask.NameToLayer("Characters")))
		{
			return false;
		}
		
		return true;
	}
	
	//Returns the angle in 2D to a specific target
	public float AngleToPoint(Vector2 target)
	{
		Vector2 position = transform.position;
		Vector2 d = target - position;
		return Mathf.Atan2 (d.y, d.x) * Mathf.Rad2Deg;
	}
}
