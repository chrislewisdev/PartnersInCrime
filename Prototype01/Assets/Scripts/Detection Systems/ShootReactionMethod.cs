using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
public class ShootReactionMethod : ReactionMethod {
	
	public float shotDelay;
	
	private FieldOfView sight;
	private float shotTimer = 0;
	
	void Start()
	{
		sight = GetComponent<FieldOfView>();	
	}
	
	void Update()
	{
		if (shotTimer > 0) shotTimer -= Time.deltaTime;
	}

	public override void OnNormal()
	{
		sight.getLight ().LightColor = Color.green;
	}
	
	public override void OnSuspicious()
	{
		sight.getLight ().LightColor = Color.yellow;
	}
	
	public override void OnAggressive()
	{
		sight.getLight ().LightColor = Color.red;
		AttackPlayer ();
	}
	
	private void AttackPlayer()
	{
		if (sight.IsObjectInView(GameManager.gameManager.Robot.gameObject))
		{
			if (shotTimer <= 0) 
			{
				GameManager.gameManager.Robot.Damage();
				shotTimer = shotDelay;
			}
		}
	}
}
