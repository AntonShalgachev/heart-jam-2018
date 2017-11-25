using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public enum Mode
	{
		Manual,
		UponCollision
	}

	public float damage;
	public bool continuous;
	public float period;
	public Mode mode;

	float delay = 0.0f;
    float multiplier = 1.0f;

	private void Update()
	{
		delay -= Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mode == Mode.UponCollision)
            TryDealDamage(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (mode == Mode.UponCollision)
            TryDealDamage(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (mode == Mode.UponCollision)
			TryDealDamage(collision.gameObject);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (mode == Mode.UponCollision)
			TryDealDamage(collision.gameObject);
	}

	void DealDamage(GameObject obj)
	{
		var health = obj.GetComponent<Health>();
		if (health)
		{
			Debug.Log(string.Format("Trying to deal {0} damage to '{1}'", damage, obj.name));
			health.TakeDamage(multiplier * damage);
		}
	}

	public void TryDealDamage(GameObject obj)
	{
		if (!continuous)
		{
			DealDamage(obj);
		}
		else if (continuous && delay < 0.0f)
		{
			DealDamage(obj);
			delay = period;
		}
	}

    public void SetMultiplier(float val)
    {
        multiplier = val;
    }
}
