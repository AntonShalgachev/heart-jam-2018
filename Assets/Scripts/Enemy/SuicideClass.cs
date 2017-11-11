using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideClass : MonoBehaviour
{
	GameObject playerInRange;
	List<GameObject> zombiesInRange = new List<GameObject>();

	GameObject movementTarget;
	Movement movement;
	Enemy enemy;

	private void Awake()
	{
		movement = GetComponent<Movement>();
		enemy = GetComponent<Enemy>();
	}

	private void Update()
	{
		movementTarget = null;

		var converted = enemy.IsConverted();

		if (playerInRange && !converted)
		{
			var direction = playerInRange.transform.position - transform.position;
			var dist = direction.magnitude;
			var layerMask = 1 << LayerMask.NameToLayer("Wall");
			var wallHit = Physics2D.Raycast(transform.position, direction.normalized, dist, layerMask);
			if (wallHit.collider == null)
				movementTarget = playerInRange;
		}
		else if (converted)
		{
			movementTarget = GetClosestZombie();
		}

		var dir = Vector2.zero;
		if (movementTarget)
			dir = movementTarget.transform.position - transform.position;

		movement.SetDirection(dir);
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

	private void OnTriggerEnter2D(Collider2D collider)
	{
#warning antonsh check components instead
		var obj = collider.gameObject;
		if (obj.layer == LayerMask.NameToLayer("Player"))
		{
			playerInRange = obj;
		}

		if (obj.layer == LayerMask.NameToLayer("Zombie"))
		{
			zombiesInRange.Add(obj);
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
#warning antonsh check components instead
		var obj = collider.gameObject;
		if (obj.layer == LayerMask.NameToLayer("Player"))
		{
			playerInRange = null;
		}

		if (obj.layer == LayerMask.NameToLayer("Zombie"))
		{
			zombiesInRange.Remove(obj);
		}
	}
}
