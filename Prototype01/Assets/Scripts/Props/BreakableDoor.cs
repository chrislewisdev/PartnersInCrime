using UnityEngine;
using System.Collections;

public class BreakableDoor : MonoBehaviour {

	public void breakDoor()
	{
		Destroy(gameObject);
	}
}
