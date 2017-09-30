using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public float maxHealth;

	float health;

	private void Start()
	{
		health = maxHealth;
	}

	public void TakeDamage(float damage)
	{
		Debug.Log(string.Format("Taking {0} damage", damage));
		health -= damage;

		if (health < 0.0f)
		{
			Destroy(gameObject);
		}
	}
}
