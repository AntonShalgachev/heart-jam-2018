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

	private void Awake()
	{
		bullets = new GameObject("Bullets");
	}

	private void Update()
	{
		shootingDelay -= Time.deltaTime;
	}

	public void TryShoot()
	{
		if (shootingDelay > 0.0f)
			return;

		var dir = transform.right;

		var spread = Random.Range(-shootingSpread, shootingSpread);
		dir = Quaternion.Euler(0, 0, spread) * dir.normalized;

		Shoot(dir);
		shootingDelay = reloadingTime;
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
}
