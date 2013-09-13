using UnityEngine;
using System.Collections.Generic;

public class AiController : AiActor {
	
	
	public bool debugControls = true;
	
	GameObject selectionCircle;
	
	void Start()
	{
		jumpToGadget(occupiedGadget);
		
		GetComponent<LineRenderer>().SetWidth(0.5f, 0.1f);
		selectionCircle = transform.GetChild(0).gameObject;
		selectionCircle.SetActive(false);
	}
	
	void Update()
	{				
		sendGadgetInput();
		
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
				jumpToGadget(closestGadget);
			
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
	
	void sendGadgetInput()
	{
		if (Input.GetButtonDown("Interact_AI"))
			sendGadgetButtonInput(ButtonState.BUTTON_DOWN);
		else if (Input.GetButtonUp("Interact_AI"))
			sendGadgetButtonInput(ButtonState.BUTTON_UP);
		else if (Input.GetButton("Interact_AI"))
			sendGadgetButtonInput(ButtonState.BUTTON_HOLD);
		else 
			sendGadgetButtonInput(ButtonState.NOT_PRESSED);
		
		sendGadgetDirectionInput(new Vector2(Input.GetAxis("Interact_Horizontal_AI"), Input.GetAxis("Interact_Vertical_AI")));
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
				jumpToGadget(closestGadget);
		}
		else
			selectionCircle.SetActive(false);
	}
	
	
	
	GameObject checkForObject(Vector2 direction)
	{
		RaycastHit hit;
		if (Physics.Raycast(new Ray(transform.position, direction), out hit))
		{
			if (hit.collider.gameObject.GetComponent<GadgetControllerInterface>() != null)
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
			if (hitObject.GetComponent<GadgetControllerInterface>() != null)
			{
				if (Physics.Raycast(new Ray(hitObject.transform.position, transform.position - hitObject.transform.position), out hit)
					&& hit.collider.gameObject == occupiedGadget)
					return hitObject;
			}
		}

		return null;
	}
}
