using UnityEngine;
using System.Collections;

// Defines if a gadget can be activated
public abstract class ActivatableGadget : MonoBehaviour {
	public abstract void activateGadget(bool triggeredByAi);
}
