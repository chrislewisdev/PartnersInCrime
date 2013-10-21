using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
public class ShootReactionMethod : ReactionMethod {
	
	public float shotDelay;
	public AudioClip shootSound;
	
	private FieldOfView sight;
	private float shotTimer = 0;
	public float ShotTimer { get { return shotTimer; } }
	
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
				//Bullet.createBullet (transform.position, GameManager.gameManager.Robot.transform.position - transform.position);
				//Debug.Break ();
				GameManager.gameManager.Robot.Damage();
				if (shootSound != null) 
					AudioSource.PlayClipAtPoint (shootSound, transform.position);
				shotTimer = shotDelay;
				Vector3 direction = GameManager.gameManager.Robot.transform.position - transform.position;
				Bullet.createBullet(transform.position, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg); 	
			}
		}
	}
}
