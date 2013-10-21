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
	private Light2D light2d;
	
	void Start()
	{
		AddLight();
		
		rotation = startingRotation;
	}
	
	public void Update()
	{
		//Utility.LogChangedValue ("EnemyInView", IsObjectInView (GameObject.Find ("AngleObject")));
		//rotation += 2f;
		
		UpdateLight();
	}
	
	//Determines whether the specified object is in view of this object.
	public bool IsObjectInView(GameObject target)
	{
		Vector3 position = target.collider.ClosestPointOnBounds (transform.position);
		
		float distance = Vector3.Distance (position, transform.position);
		if (distance > range) return false;
		
		//Calculate the angle between our viewpoint and the target
		//Make sure we're Z-aligned so the angle is purely in 2D space
		float deltaAngle = Mathf.Abs (Mathf.DeltaAngle (rotation, AngleToPoint (position)));
		if (deltaAngle > fieldOfView / 2) return false;
		
		//If target is both in range and in view, check that line of sight is not blocked
		Ray lineOfSight = new Ray(transform.position, position - transform.position);
		RaycastHit hitInfo;
		//Debug.DrawRay (transform.position, position - transform.position);
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
	
	//Sets rotation of fieldOfView
	public void setRotation(float angle)
	{
		rotation = angle;
	}
	
	public Light2D getLight()
	{
		return light2d;
	}
	
	private void AddLight()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			light2d = transform.GetChild(i).gameObject.GetComponent<Light2D>();
			if (light2d)
				return;
		}
		
		light2d = Light2D.Create(transform.position, Color.red, range, (int)fieldOfView); 
		light2d.gameObject.transform.parent = transform;
	}
	
	private void UpdateLight()
	{
		light2d.gameObject.transform.eulerAngles = new Vector3(0f, 0f, rotation);
	}
}
