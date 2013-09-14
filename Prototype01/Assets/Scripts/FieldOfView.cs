using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {
	
	//Editor variables
	public float fieldOfView;
	public float range;
	public Material material;
	public int startingRotation;
	
	private float rotation = 0f;
	public float Rotation { get { return rotation; } set { rotation = value; } }
	private LineRenderer lineRenderer;
	
	void Start()
	{
		//Make sure that a LineRenderer is attached to this object
		lineRenderer = GetComponent<LineRenderer>();
		if (lineRenderer == null)
			lineRenderer = gameObject.AddComponent<LineRenderer>();
		//Set up the renderer
		lineRenderer.SetVertexCount(5);
		lineRenderer.useWorldSpace = false;
		lineRenderer.SetWidth (0.2f, 0.2f);
		lineRenderer.material = material;
		
		rotation = startingRotation;
	}
	
	public void Update()
	{
		//Utility.LogChangedValue ("EnemyInView", IsObjectInView (GameObject.Find ("AngleObject")));
		UpdateLineRenderer();
		//rotation += 2f;
	}
	
	//Determines whether the specified object is in view of this object.
	public bool IsObjectInView(GameObject target)
	{
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if (distance > range) return false;
		
		//Calculate the angle between our viewpoint and the target
		//Make sure we're Z-aligned so the angle is purely in 2D space
		float deltaAngle = Mathf.Abs (Mathf.DeltaAngle (rotation, AngleToPoint (target.transform.position)));
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
	
	private void UpdateLineRenderer()
	{
		//Re-calculate our line points
		float angle1 = rotation - fieldOfView/2, angle2 = rotation + fieldOfView/2;
		Vector3 p1 = new Vector3(Mathf.Cos (angle1 * Mathf.Deg2Rad) * range, Mathf.Sin (angle1 * Mathf.Deg2Rad) * range, -1);
		Vector3 p2 = new Vector3(Mathf.Cos (rotation * Mathf.Deg2Rad) * range, Mathf.Sin (rotation * Mathf.Deg2Rad) * range, -1);
		Vector3 p3 = new Vector3(Mathf.Cos (angle2 * Mathf.Deg2Rad) * range, Mathf.Sin (angle2 * Mathf.Deg2Rad) * range, -1);
		//Update our line points
		lineRenderer.SetPosition(0, Vector3.zero + Vector3.back);
		lineRenderer.SetPosition(1, p1);
		lineRenderer.SetPosition(2, p2);
		lineRenderer.SetPosition(3, p3);
		lineRenderer.SetPosition(4, Vector3.zero + Vector3.back);
	}
}
