using UnityEngine;
using System.Collections;

//Causes an objects scale to pulse
public class Pulse : MonoBehaviour {

	public float amplitude;
	public float speed;
	
	private float timer;
	private Vector3 startingScale;
	
	void Start()
	{
		startingScale = transform.localScale;
	}
	
	void Update()
	{
		float newX = Mathf.Sin((timer += Time.deltaTime * speed)) * amplitude;
		transform.localScale = startingScale + new Vector3(newX, newX, newX);
	}
}
