using UnityEngine;
using System.Collections.Generic;

public class AiController : MonoBehaviour {
	
	public GameObject occupiedGadget;
	public GameObject selectionCircle;
	static float SELECTION_THRESHOLD = .5f;
	
	
	void Start()
	{
		jumpToGadget(occupiedGadget);
		GetComponent<LineRenderer>().SetWidth(0.5f, 0.1f);
		selectionCircle.SetActive(false);
	}
	
	void Update()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			jumpToGadget(GameObject.FindGameObjectWithTag("Player"));
			return;
		}
		
		Vector3 input = new Vector3(Input.GetAxis("Horizontal_AI"), Input.GetAxis("Vertical_AI"), 0f);
		if (input.magnitude > 0.2f)
		{
			// Horrifically unoptimized but hey its a prototype :P 
			GameObject closestGadget = snapToClosestGadget(Mathf.Atan2(input.y, input.x), getVisibleGadgets());
			if (closestGadget && Input.GetButtonDown("Jump_AI"))
				jumpToGadget(closestGadget);
			
			LineRenderer renderer = GetComponent<LineRenderer>();
			renderer.enabled = true;
			renderer.SetPosition(0, transform.position);
			renderer.SetPosition(1, transform.position + input.normalized * 6.0f);
			
			if (closestGadget)
			{
				selectionCircle.SetActive(true);
				selectionCircle.transform.position = closestGadget.transform.position;
			}
			else
			{
				selectionCircle.SetActive(false);
			}
		}
		else
		{
			GetComponent<LineRenderer>().enabled = false;
			selectionCircle.SetActive(false);
		}
	}
	
	void jumpToGadget(GameObject gadget)
	{
		occupiedGadget = gadget;
		transform.parent = occupiedGadget.transform;
		transform.position = occupiedGadget.transform.position;
	}
	
	List<GameObject> getVisibleGadgets()
	{
		GameObject[] gadgets = GameObject.FindGameObjectsWithTag("AI_Controllable");
		List<GameObject> visibleObjects = new List<GameObject>();
		
		foreach (GameObject g in gadgets)
		{
			if (g != occupiedGadget && !Physics.Raycast(new Ray(transform.position, g.transform.position - transform.position), Vector3.Distance(transform.position, g.transform.position)))
				visibleObjects.Add(g);
		}
		
		return visibleObjects;
	}
	
	//Finds gadget at closest angle to inputAngle and returns the object
	GameObject snapToClosestGadget(float inputAngle, List<GameObject> visibleGadgets)
	{
		float[] angles = new float[visibleGadgets.Count];
		for (int i = 0; i < visibleGadgets.Count; ++i)
			angles[i] = Mathf.Atan2(visibleGadgets[i].transform.position.y - transform.position.y, 
				visibleGadgets[i].transform.position.x - transform.position.x);
	
		GameObject closestGadget = null;
		float smallestAngle = float.MaxValue;
		for (int i = 0; i < visibleGadgets.Count; ++i)
		{
			float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(inputAngle * Mathf.Rad2Deg, angles[i] * Mathf.Rad2Deg) * Mathf.Deg2Rad);
			if (deltaAngle < smallestAngle)
			{
				smallestAngle = deltaAngle;
				closestGadget = visibleGadgets[i];
			}
		}
		
		if (smallestAngle < SELECTION_THRESHOLD)
			return closestGadget;
		else
			return null;
	}
}
