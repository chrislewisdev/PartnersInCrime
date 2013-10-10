using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Bullet : MonoBehaviour {
	
	private static float ACCURACY = 2.5f;
	private static float HIT_CHANCE = 0.2f;
	private static GameObject bulletPrefab = Resources.Load("bullet") as GameObject;
	private float disappearTimer = .05f;
	
	public static void createBullet(Vector3 origin, Vector3 direction)
	{
		createBullet(origin, Mathf.Atan2(direction.y, direction.x));
	}
	
	public static void createBullet(Vector3 origin, float angle)
	{
		angle += Random.Range(-1.0f, 1.0f) * ACCURACY;
		
		Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
		createBulletandInit(origin, direction);
	}
	
	private static void createBulletandInit(Vector3 origin, Vector3 direction)
	{
		GameObject newBullet = Instantiate(bulletPrefab, origin + new Vector3(0f, 0f, .1f), Quaternion.identity) as GameObject;
		newBullet.GetComponent<Bullet>().initBullet(direction);
	}
	
	private void initBullet(Vector3 direction)
	{
		RaycastHit hit;
		if (Physics.Raycast(new Ray(transform.position, direction), out hit))
		{
			RobotController robot = hit.collider.GetComponent<RobotController>();
			if (robot != null && Random.Range(0f, 1.0f) < HIT_CHANCE)
				robot.Damage();
			
			GuardController guard = hit.collider.GetComponent<GuardController>();
			if (guard != null)
				guard.Damage(1);
		
			LineRenderer lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, hit.point);
		}
	}
	
	void Update()
	{
		disappearTimer -= Time.deltaTime;
		if (disappearTimer <= 0.0f)
			Destroy(gameObject);
	}
}
