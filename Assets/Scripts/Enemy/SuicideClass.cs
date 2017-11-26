using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class SuicideClass : MonoBehaviour
{
	public float meleeDistance;

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

#warning antonsh extract this repetitive code
		if (agressive && playerInRange)
		{
			var direction = playerInRange.transform.position - transform.position;
			var dist = direction.magnitude;
			var layerMask = 1 << LayerMask.NameToLayer("Wall");
			var wallHit = Physics2D.Raycast(transform.position, direction.normalized, dist, layerMask);
			if (wallHit.collider == null)
				target = playerInRange;
		}
		else if (converted || (agressive && playerInRange == null))
		{
			target = GetClosestZombie();
		}

		var dir = Vector2.zero;
        if (target)
        {
            dir = target.transform.position - transform.position;
        }
        else
        {
            if(WayPointManager.Instance.onTheWay(transform.position))
                dir = WayPointManager.Instance.getNextPoint(transform.position, movement.GetDirection());
        }
		movement.SetDirection(dir);

		if (target && DistanceTo(target) < meleeDistance)
			Attack(target);
	}

	GameObject GetClosestZombie()
	{
		GameObject closestZombie = null;

		foreach (var zombie in zombiesInRange)
		{
			if (zombie == null)
				continue;

			var otherEnemy = zombie.GetComponent<Enemy>();
			if (!otherEnemy)
				continue;

			var selfConverted = enemy.IsConverted();
			var selfAgressive = enemy.IsAgressive();
			var otherConverted = otherEnemy.IsConverted();
			var otherAgressive = otherEnemy.IsAgressive();

			if (selfAgressive && otherAgressive) // if we are agressive, skip other agressive
				continue;
			if (selfConverted && otherConverted) // if we are converted, skip other converted
				continue;
			if (!otherConverted && !otherAgressive) // skip passive NPC
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

	float DistanceTo(GameObject obj)
	{
		return (obj.transform.position - transform.position).magnitude;
	}

	void Attack(GameObject obj)
	{
		if (attack)
			attack.TryDealDamage(obj);
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
