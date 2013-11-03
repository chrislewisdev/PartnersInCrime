using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FieldOfView))]
public class ShootReactionMethod : ReactionMethod {
	
	public float shotDelay;
	public AudioClip shootSound;
	
	private FieldOfView sight;
	private float shotTimer = 0;
	public float ShotTimer { get { return shotTimer; } }
	private PositionMarker gunPosition;
	
	void Start()
	{
		sight = GetComponent<FieldOfView>();
		gunPosition = GetComponentInChildren<PositionMarker>();
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
				if (shootSound != null) 
					AudioSource.PlayClipAtPoint (shootSound, transform.position);
				shotTimer = shotDelay;
				if (gunPosition == null)
				{
					Vector3 direction = GameManager.gameManager.Robot.transform.position - transform.position;
					Bullet.createBullet(transform.position, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, GameManager.gameManager.Robot.gameObject);
				}
				else
				{
					Vector3 direction = GameManager.gameManager.Robot.transform.position - gunPosition.transform.position;
					Bullet.createBullet(gunPosition.transform.position, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, GameManager.gameManager.Robot.gameObject);
				}
			}
		}
	}
}
