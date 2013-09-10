using UnityEngine;
using System.Collections;

public class AiActor : MonoBehaviour {

	public GameObject occupiedGadget;
	
	public void jumpToGadget(GameObject gadget)
	{
		transform.parent = gadget.transform;
		transform.localPosition = Vector3.zero;
		occupiedGadget = gadget;
	}
	
	void Start()
	{
		jumpToGadget(occupiedGadget);
	}
}
