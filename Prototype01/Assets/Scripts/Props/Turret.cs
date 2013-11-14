using UnityEngine;
using System.Collections;

public class Turret : AiControllable {
	
	public AudioClip shootSound;
	
	// If true, connected gadgets to the turret will only be triggered when the ai fires the turret, otherwise they will be triggered whenever the turret fires
	public bool triggerGadgetsOnlyOnAiFire = true;
	
	private static float reloadTimer = .1f;
	private float fireTimer = 0f;
	private GameObject controllerButton;
	
	public override void activateGadget (bool triggeredByAi)
	{
		fireTurret();
		
		if (!triggeredByAi)
		{
			Vector3 playerDirection = GameManager.gameManager.Robot.gameObject.transform.position - transform.position;
			float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0f, 0f, angle);
		}
		
		if (triggerGadgetsOnlyOnAiFire)
		{
			if (triggeredByAi)
				activateConnectedGadgets();
		}
		else
			activateConnectedGadgets();
	}
	
	public override void aiSendDirection (Vector2 direction)
	{
		if (direction.magnitude > 0.5f)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0f, 0f, angle);
		}
	}
	
	public override void aiArrived ()
	{
		controllerButton = Instantiate(Resources.Load("X Button") as GameObject, transform.position + new Vector3(0f, 2f, -1f), Quaternion.identity) as GameObject;
	}
	
	public override void aiLeft ()
	{
		if (controllerButton)
		{
			Destroy(controllerButton);
			controllerButton = null;
		}	
	}
	
	private void fireTurret()
	{	
		if (fireTimer >= reloadTimer && !GameManager.gameManager.Robot.isDestroyed())
		{
			if (shootSound != null) AudioSource.PlayClipAtPoint(shootSound, transform.position);
			Bullet.createBullet(transform.position, transform.eulerAngles.z);
			fireTimer = 0f;	
		}
	}
	
	void Update()
	{
		fireTimer += Time.deltaTime;
	}
}
