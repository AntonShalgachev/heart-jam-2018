using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float damage;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		DealDamage(collision.gameObject);
	}

	void DealDamage(GameObject obj)
	{
		Debug.Log(string.Format("Trying to deal {0} damage to '{1}'", damage, obj.name));

		var health = obj.GetComponent<Health>();
		if (health)
			health.TakeDamage(damage);
	}
}
