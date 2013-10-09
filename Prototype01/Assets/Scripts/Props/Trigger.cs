using UnityEngine;
using System.Collections;

public class Trigger : GadgetInterface {

	public override void activateGadget (bool triggeredByAi)
	{
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.GetComponent<RobotController>() != null)
			activateConnectedGadgets();
	}
}
