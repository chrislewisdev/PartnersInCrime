using UnityEngine;
using System.Collections.Generic;

//Defines if a gadget can be actiavated as well as activate other objects
public abstract class GadgetInterface : ActivatableGadget {
	public List<ActivatableGadget> triggerGadgets = new List<ActivatableGadget>();
	
	protected void activateConnectedGadgets()
	{
		foreach (ActivatableGadget gadget in triggerGadgets)
		{
			gadget.activateGadget(false);
		}
	}
	
	
}
