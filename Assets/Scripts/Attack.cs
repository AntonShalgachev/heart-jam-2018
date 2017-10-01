using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float damage;
	public bool continuous;
	public float period;

	float delay = 0.0f;

	private void Update()
	{
		delay -= Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!continuous)
		{
			DealDamage(collision.gameObject);
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (continuous && delay < 0.0f)
		{
			DealDamage(collision.gameObject);
			delay = period;
		}
	}

	void DealDamage(GameObject obj)
	{
		var health = obj.GetComponent<Health>();
		if (health)
		{
			//Debug.Log(string.Format("Trying to deal {0} damage to '{1}'", damage, obj.name));
			health.TakeDamage(damage);
		}
	}
}
