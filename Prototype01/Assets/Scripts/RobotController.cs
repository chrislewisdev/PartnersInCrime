using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		if (Mathf.Abs (horizontal) > 0.1f)
		{
			if (horizontal > 0.1f && rigidbody.velocity.x < 15f ||
				horizontal < 0.1f && rigidbody.velocity.x > -15f)
			{
				rigidbody.AddForce (-Vector3.left * horizontal * 500f);
			}
		}
		else if (Mathf.Abs (rigidbody.velocity.x) > 1f)
		{
			rigidbody.AddForce (Vector3.left * rigidbody.velocity.x * 10);
		}
		
		if (Input.GetKeyDown (KeyCode.UpArrow) && OnGround ())
		{
			rigidbody.AddForce (Vector3.up * 1000f);
		}
	}
	
	private bool OnGround()
	{
		return Physics.Raycast (new Ray(transform.position, Vector3.down), 1.7f);
	}
}
