using UnityEngine;
using System.Collections;

public abstract class Spawner : ActivatableGadget {

	public abstract void spawn();
	
	public override void activateGadget (bool triggeredByAi)
	{
		spawn ();
	}
}
