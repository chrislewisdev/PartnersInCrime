using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour {
	
	CharacterController moveController;
	Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		moveController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		/*if (Mathf.Abs (horizontal) > 0.1f)
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
		}*/
		
		//Vector3 acceleration = -Vector3.left * horizontal;
		//acceleration += Physics.gravity;
		
		//velocity = Physics.gravity;
		velocity = moveController.velocity;
		velocity.x = horizontal * 15f;
		
		if (Input.GetKeyDown (KeyCode.UpArrow) && moveController.isGrounded)
		{
			velocity += Vector3.up * 35f;
		}
		
		velocity += Physics.gravity * Time.deltaTime * 2;
		if (velocity.y < Physics.gravity.y)
		{
			velocity.y = Physics.gravity.y;
		}
		
		moveController.Move (velocity * Time.deltaTime);
	}
	
	private bool OnGround()
	{
		return Physics.Raycast (new Ray(transform.position, Vector3.down), 1.7f);
	}
}
