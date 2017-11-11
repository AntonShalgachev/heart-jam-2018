using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
	public float shootingSpread;
	public float reloadingTime;
	public float bulletSpeed;
	public GameObject bulletPrefab;

	float shootingDelay = 0.0f;
	GameObject bullets;
	List<GameObject> zombiesInRange;

	private void Awake()
	{
		bullets = new GameObject("Bullets");
		zombiesInRange = new List<GameObject>();
	}

	private void Update()
	{
		shootingDelay -= Time.deltaTime;
	}

	public void TryShoot()
	{
		if (shootingDelay > 0.0f)
			return;

#warning antonsh finding closest zombie only for tests
		var zombie = GetClosestZombie();
		if (zombie == null)
			return;

		var targetDir = (Vector2)zombie.transform.position - (Vector2)transform.position;
		var distance = targetDir.magnitude;

		var spread = Random.Range(-shootingSpread, shootingSpread);
		var dir = Quaternion.Euler(0, 0, spread) * targetDir.normalized;

		Shoot(dir);
		shootingDelay = reloadingTime;
	}

	GameObject GetClosestZombie()
	{
		GameObject closestZombie = null;

		foreach (var zombie in zombiesInRange)
		{
			if (zombie == null)
				continue;

			var dir = zombie.transform.position - transform.position;
			var dist = dir.magnitude;
			var layerMask = 1 << LayerMask.NameToLayer("Wall");
			var wallHit = Physics2D.Raycast(transform.position, dir.normalized, dist, layerMask);
			if (wallHit.collider != null)
				continue;

			if (closestZombie == null)
			{
				closestZombie = zombie;
			}
			else
			{
				var closestDist = (transform.position - closestZombie.transform.position).magnitude;
				if (dist < closestDist)
					closestZombie = zombie;
			}
		}

		return closestZombie;
	}

	void Shoot(Vector2 dir)
	{
		var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bullets.transform);
		bullet.name = "Bullet";

		var body = bullet.GetComponent<Rigidbody2D>();
		Debug.Assert(body);

		body.AddForce(dir * bulletSpeed, ForceMode2D.Force);

		bullet.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		var zombie = collider.gameObject;
		if (zombie.layer == LayerMask.NameToLayer("Zombie"))
		{
			zombiesInRange.Add(zombie);
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		var zombie = collider.gameObject;
		if (zombie.layer == LayerMask.NameToLayer("Zombie"))
		{
			zombiesInRange.Remove(zombie);
		}
	}
}
