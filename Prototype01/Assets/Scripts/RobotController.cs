﻿using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour {
	
	//Editor variables
	public float speed;
	public float jumpStrength;
	public float gravityFactor;
	
	CharacterController moveController;
	Vector3 velocity = Vector3.zero;
	private bool jumped = false;

	// Use this for initialization
	void Start () {
		moveController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		//velocity = Physics.gravity;
		velocity = moveController.velocity;
		velocity.x = horizontal * speed;
		
		if (vertical > 0 && !jumped)
		{
			if (moveController.isGrounded)
			{
				velocity += Vector3.up * jumpStrength;
			}
			jumped = true;
		}
		else if (vertical <= 0)
		{
			jumped = false;
		}
		
		velocity += Physics.gravity * Time.deltaTime * gravityFactor;
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
