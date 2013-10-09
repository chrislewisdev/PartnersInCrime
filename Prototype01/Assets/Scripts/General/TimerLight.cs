using UnityEngine;
using System.Collections;

//Controls light2d that slowly reveals itself in tune with a timer then destroys itself at the end
public class TimerLight : MonoBehaviour {

	public float revealTime;
	
	private Light2D light2d;
	private float timer;
	
	void Start()
	{
		light2d = GetComponent<Light2D>();
		timer = 0f;
	}
	
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > revealTime )
		{
			Destroy(gameObject);
			return;
		}
		
		light2d.LightConeAngle = Mathf.Lerp(0f, 360f, timer / revealTime);
	}
}
