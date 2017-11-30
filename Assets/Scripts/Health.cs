using Assets.Scripts.UI;
using UnityEngine;

public class Health : PropertyChanger
{
	public float maxHealth;
	public bool regen;
	public float regenRate;
    public bool needBar = true;

    float health;
    bool bHsaHPbar;
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
        if(health < maxHealth && !bHsaHPbar && needBar)
        {
            bHsaHPbar = true;
            UIManager.Instance.createHPBar(transform);
        }

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
