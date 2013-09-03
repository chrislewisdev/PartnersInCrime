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
				selectionCircle.transform.localPosition = Vector3.zero;
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
}
