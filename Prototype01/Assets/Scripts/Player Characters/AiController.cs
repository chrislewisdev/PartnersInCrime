﻿using UnityEngine;
using System.Collections.Generic;

public class AiController : AiActor {
	
	
	public bool debugControls = true;
	public AudioClip transitionSound;
	public float maxJumpRange = 50.0f;
	public float maxRobotDistance = 100.0f;
	
	GameObject selectionCircle;
	
	// Returns selection circle object or null if it is not currently active
	public GameObject getSelectionCircle()
	{
		if (selectionCircle.activeSelf)
			return selectionCircle;
		else
			return null;
	}
	
	void Start()
	{
		initilize();
		jumpToGadget(occupiedGadget);
		occupiedGadget.GetComponent<AiControllable>().aiArrived();
		
		GetComponent<LineRenderer>().SetWidth(0.5f, 0.1f);
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.name == "SelectionCircle")
			{
				selectionCircle = transform.GetChild(i).gameObject;
				break;	
			}
		}
		
		selectionCircle.SetActive(false);
	}
	
	void Update()
	{				
		updateMovement();
		sendGadgetInput();
		
		if (debugControls)
		{
			debugControlsUpdate();
			return;	
		}
	
		Vector3 input = new Vector3(Input.GetAxis("Horizontal_AI"), Input.GetAxis("Vertical_AI"), 0f);
		if (input.magnitude > 0.2f)
		{
			GameObject closestGadget = snapToClosestGadget(Mathf.Atan2(input.y, input.x));
			if (closestGadget && Input.GetButtonDown("Jump_AI"))
			{
				occupiedGadget.GetComponent<AiControllable>().aiLeft();
				jumpToGadget(closestGadget);
				//Play our transition sound
				if (transitionSound != null) AudioSource.PlayClipAtPoint (transitionSound, transform.position);
				closestGadget.GetComponent<AiControllable>().aiArrived();
			}
			
			LineRenderer renderer = GetComponent<LineRenderer>();
			renderer.enabled = true;
			renderer.SetPosition(0, transform.position);
			renderer.SetPosition(1, transform.position + input.normalized * 6.0f);
			
			if (closestGadget)
			{
				selectionCircle.SetActive(true);
				selectionCircle.transform.position = closestGadget.transform.position + new Vector3(0f, 0f, -1f);
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
		
		sendGadgetDirectionInput(new Vector2(Input.GetAxis("Horizontal_AI"), Input.GetAxis("Vertical_AI")));
	}
	
	void debugControlsUpdate()
	{
		GameObject closestGadget = objectClosestToMouse();
		if (closestGadget)
		{
			selectionCircle.SetActive(true);
			selectionCircle.transform.position = closestGadget.transform.position + new Vector3(0f, 0f, -1f);
			
			if (Input.GetMouseButtonDown(0))
			{
				occupiedGadget.GetComponent<AiControllable>().aiLeft();
				jumpToGadget(closestGadget);
				if (transitionSound != null) AudioSource.PlayClipAtPoint (transitionSound, transform.position);
				occupiedGadget.GetComponent<AiControllable>().aiArrived();
			}
		}
		else
			selectionCircle.SetActive(false);
	}
	
	
	
	/*GameObject checkForObject(Vector2 direction)
	{
		RaycastHit hit;
		/*Vector3 position = transform.position;
		position.z = 0f;*
		
		if (Physics.Raycast(new Ray(transform.position, direction), out hit))
		{
			if (hit.collider.gameObject.GetComponent<AiControllable>() != null)
				return hit.collider.gameObject;
		}
		
		return null;
	}*/
	
	GameObject objectClosestToMouse()
	{
		Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<tk2dCamera>().camera;
		RaycastHit hit;
		
		if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
		{
			GameObject hitObject = hit.collider.gameObject;
			
			if (hitObject.GetComponent<AiControllable>() != null)
			{		
				Debug.DrawRay (occupiedGadget.transform.position, hitObject.transform.position - occupiedGadget.transform.position, Color.red);
				//if (Input.GetKey(KeyCode.D)) Debug.Break ();
				if (Physics.Raycast(new Ray(occupiedGadget.transform.position, hitObject.transform.position - occupiedGadget.transform.position), out hit) || Vector3.Distance(transform.position, hitObject.transform.position) < 6.4f)
				{	
					if (hit.collider && hit.collider.gameObject == hitObject)
						return hitObject;
				}
			}
		}

		return null;
	}
	
	List<GameObject> getVisibleGadgets()
	{
		AiControllable[] gadgets = (AiControllable[])Object.FindObjectsOfType(typeof(AiControllable));
		List<GameObject> visibleObjects = new List<GameObject>();
		
		foreach (AiControllable g in gadgets)
		{
			if (g.gameObject != occupiedGadget && 
				Vector3.Distance(g.gameObject.transform.position, GameManager.gameManager.Robot.transform.position) < maxRobotDistance)
			{
				RaycastHit hit;
				//Debug.DrawRay (occupiedGadget.transform.position, g.gameObject.transform.position - occupiedGadget.transform.position, Color.red);
				Physics.Raycast(new Ray(occupiedGadget.transform.position, g.gameObject.transform.position - occupiedGadget.transform.position), out hit);
				if ((hit.collider.gameObject == g.gameObject && Vector3.Distance(transform.position, g.gameObject.transform.position) < maxJumpRange)
					|| Vector3.Distance(transform.position, g.transform.position) < 6.2f)
					visibleObjects.Add(g.gameObject);
				else if (g.GetComponent<Door>()) // if door, do additional raycasts
				{
					if (g.transform.rotation.z == 0.0f) // if vertical door
					{
						if ((Physics.Raycast(new Ray(occupiedGadget.transform.position, 
							g.gameObject.transform.position + new Vector3(g.collider.bounds.extents.x, 0f, 0f) - occupiedGadget.transform.position), out hit) 
							&& hit.collider.gameObject == g.gameObject) ||
							(Physics.Raycast(new Ray(occupiedGadget.transform.position, 
							g.gameObject.transform.position - new Vector3(g.collider.bounds.extents.x, 0f, 0f) - occupiedGadget.transform.position), out hit)) 
							&& hit.collider.gameObject == g.gameObject)
						{
							visibleObjects.Add(g.gameObject);
						}
					}
					else // if horizontal door
					{
						if ((Physics.Raycast(new Ray(occupiedGadget.transform.position, 
							g.gameObject.transform.position + new Vector3(0f, g.collider.bounds.extents.y, 0f) - occupiedGadget.transform.position), out hit) 
							&& hit.collider.gameObject == g.gameObject) ||
							(Physics.Raycast(new Ray(occupiedGadget.transform.position, 
							g.gameObject.transform.position - new Vector3(0f, g.collider.bounds.extents.y, 0f) - occupiedGadget.transform.position), out hit)) 
							&& hit.collider.gameObject == g.gameObject)
						{
							visibleObjects.Add(g.gameObject);
						}
					}
				}
			}
		}
		
		return visibleObjects;
	}
	
	//Finds gadget at closest angle to inputAngle and returns the object
	GameObject snapToClosestGadget(float inputAngle)
	{
		List<GameObject> visibleGadgets = getVisibleGadgets();
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
		
		if (smallestAngle < 45f * Mathf.Deg2Rad)
			return closestGadget;
		else
			return null;
	}
}
