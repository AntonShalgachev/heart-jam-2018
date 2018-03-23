using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentClass : MonoBehaviour
{
	public float triggerDistance;

	GameObject playerInRange;
	List<GameObject> zombiesInRange = new List<GameObject>();

	GameObject target;
	Movement movement;
	Enemy enemy;
	Attack attack;

	private void Awake()
	{
		movement = GetComponent<Movement>();
		enemy = GetComponent<Enemy>();
		attack = GetComponent<Attack>();
	}

	private void Update()
	{
		target = null;

		var converted = enemy.IsConverted();
		var agressive = enemy.IsAgressive();

		//#warning antonsh extract this repetitive code
		//		if (agressive && playerInRange)
		//		{
		//			var direction = playerInRange.transform.position - transform.position;
		//			var dist = direction.magnitude;
		//			var layerMask = 1 << LayerMask.NameToLayer("Wall");
		//			var wallHit = Physics2D.Raycast(transform.position, direction.normalized, dist, layerMask);
		//			if (wallHit.collider == null)
		//				target = playerInRange;
		//		}
		//		else if (converted)
		//		{
		//			target = GetClosestZombie();
		//		}

		target = GetClosestNonAgnetEnemy();

		var dir = Vector2.zero;
		if (target)
			dir = target.transform.position - transform.position;

		movement.SetDirection(dir);

		if (target && DistanceTo(target) < triggerDistance)
			MakeAgressive(target);
	}

	GameObject GetClosestNonAgnetEnemy()
	{
		GameObject closestZombie = null;

		foreach (var zombie in zombiesInRange)
		{
			if (zombie == null)
				continue;

			var enemy = zombie.GetComponent<Enemy>();
			if (!enemy || enemy.IsConverted() || enemy.IsAgressive() || !enemy.canBeAgressive || !enemy.isConvertible)
				continue;

			//if (zombie.GetComponent<AgentClass>())
			//	continue;

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

	float DistanceTo(GameObject obj)
	{
		return (obj.transform.position - transform.position).magnitude;
	}

	void MakeAgressive(GameObject obj)
	{
		var objEnemy = obj.GetComponent<Enemy>();
		if (objEnemy)
			objEnemy.TryMakeAgressive();
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
