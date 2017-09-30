using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovementAI : MonoBehaviour
{
	GameObject playerInRange;
	GameObject visiblePlayer;
	Movement movement;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void Update()
	{
		visiblePlayer = null;
		if (playerInRange)
		{
			var direction = playerInRange.transform.position - transform.position;
			var dist = direction.magnitude;
			var layerMask = 1 << LayerMask.NameToLayer("Wall");
			var wallHit = Physics2D.Raycast(transform.position, direction.normalized, dist, layerMask);
			if (wallHit.collider == null)
				visiblePlayer = playerInRange;
		}

		var dir = Vector2.zero;
		if (visiblePlayer)
			dir = visiblePlayer.transform.position - transform.position;

		movement.SetDirection(dir);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		var obj = collider.gameObject;
		if (obj.layer == LayerMask.NameToLayer("Player"))
		{
			playerInRange = obj;
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		var obj = collider.gameObject;
		if (obj.layer == LayerMask.NameToLayer("Player"))
		{
			playerInRange = null;
		}
	}
}
