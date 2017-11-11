using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public event Action onSpawnReached;
	public Converter converter;

	Movement movement;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void Update()
	{
		var dir = GetCurrentDirection();
		movement.SetDirection(dir);

		if (Input.GetMouseButtonDown(1))
			converter.TryShoot();
	}

	Vector2 GetCurrentDirection()
	{
		var dir = GetWASDDirection();

		return dir;
	}

	Vector2 GetWASDDirection()
	{
		Vector2 dir = Vector2.zero;

		if (Input.GetKey(KeyCode.W))
			dir += new Vector2(0.0f, 1.0f);
		if (Input.GetKey(KeyCode.S))
			dir += new Vector2(0.0f, -1.0f);
		if (Input.GetKey(KeyCode.A))
			dir += new Vector2(-1.0f, 0.0f);
		if (Input.GetKey(KeyCode.D))
			dir += new Vector2(1.0f, 0.0f);

		return dir;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerSpawn"))
		{
			if (onSpawnReached != null)
				onSpawnReached.Invoke();
		}
	}
}
