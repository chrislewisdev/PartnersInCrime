using UnityEngine;
using System.Collections.Generic;

public class AiController : MonoBehaviour {
	
	
	public bool debugControls = true;
	
	GameObject selectionCircle;
	AiActor aiActor;
	
	void Start()
	{
		GetComponent<LineRenderer>().SetWidth(0.5f, 0.1f);
		selectionCircle = transform.GetChild(0).gameObject;
		selectionCircle.SetActive(false);
		
		aiActor = GetComponent<AiActor>();
	}
	
	
	
	void Update()
	{				
		if (debugControls)
		{
			debugControlsUpdate();
			return;	
		}
	
		Vector3 input = new Vector3(Input.GetAxis("Horizontal_AI"), Input.GetAxis("Vertical_AI"), 0f);
		if (input.magnitude > 0.2f)
		{
			GameObject closestGadget = checkForObject(input);
			if (closestGadget && Input.GetButtonDown("Jump_AI"))
				aiActor.jumpToGadget(closestGadget);
			
			LineRenderer renderer = GetComponent<LineRenderer>();
			renderer.enabled = true;
			renderer.SetPosition(0, transform.position);
			renderer.SetPosition(1, transform.position + input.normalized * 6.0f);
			
			if (closestGadget)
			{
				selectionCircle.SetActive(true);
				selectionCircle.transform.parent = closestGadget.transform;
				selectionCircle.transform.localPosition = new Vector3(0f, 0f, -1f);
			}
			else
				selectionCircle.SetActive(false);	
		}
		else
		{
			GetComponent<LineRenderer>().enabled = false;
			selectionCircle.SetActive(false);
		}
	}
	
	void debugControlsUpdate()
	{
		GameObject closestGadget = objectClosestToMouse();
		if (closestGadget)
		{
			selectionCircle.SetActive(true);
			selectionCircle.transform.parent = closestGadget.transform;
			selectionCircle.transform.localPosition = new Vector3(0f, 0f, -1f);
			
			if (Input.GetMouseButtonDown(0))
				aiActor.jumpToGadget(closestGadget);
		}
		else
			selectionCircle.SetActive(false);
	}
	
	
	
	GameObject checkForObject(Vector2 direction)
	{
		RaycastHit hit;
		if (Physics.Raycast(new Ray(transform.position, direction), out hit))
		{
			if (hit.collider.gameObject.tag == "AI_Controllable" || hit.collider.gameObject.tag == "Player")
				return hit.collider.gameObject;
		}
		
		return null;
	}
	
	GameObject objectClosestToMouse()
	{
		Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<tk2dCamera>().camera;
		RaycastHit hit;
		
		if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
		{
			GameObject hitObject = hit.collider.gameObject;
			if (hitObject.tag == "AI_Controllable" || hitObject.tag == "Player")
			{
				if (Physics.Raycast(new Ray(transform.position, hitObject.transform.position - transform.position), out hit)
					&& hit.collider.gameObject == hitObject)
					return hit.collider.gameObject;
			}
		}

		return null;
	}
}
