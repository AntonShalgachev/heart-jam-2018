using Assets.Scripts.UI;
using UnityEngine;

public class Health : PropertyChanger
{
	public float maxHealth;
	public bool regen;
	public float regenRate;

    float health;

	private void Awake()
	{
		health = maxHealth;
	}

	private void Update()
	{
		if (regen)
			RegenerateHealth(regenRate* Time.deltaTime);
	}

	public void TakeDamage(float damage)
	{
		health = Mathf.Clamp(health - damage, 0.0f, maxHealth);
        OnPropertyChanged("Health");

        if (Mathf.Abs(health) < Mathf.Epsilon)
		{
			Destroy(gameObject);
		}
	}

	public void RegenerateHealth(float hp)
	{
		health = Mathf.Clamp(health + hp, 0.0f, maxHealth);
        OnPropertyChanged("Health");
    }

	public float GetHealth()
	{
		return health;
	}

	public float GetMaxHealth()
	{
		return maxHealth;
	}
}
