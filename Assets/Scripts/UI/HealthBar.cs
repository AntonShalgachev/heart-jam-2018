using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Text healthText;
	public string healthTextString;

	Health playerHealth;
	Slider slider;

	private void Awake()
	{
		slider = GetComponent<Slider>();
	}

	private void Update()
	{
		if (playerHealth)
		{
			var health = playerHealth.GetHealth();
			var maxHealth = playerHealth.GetMaxHealth();

			var ratio = health / maxHealth;
			slider.value = ratio;

			healthText.text = string.Format(healthTextString, (int)health, (int)maxHealth);
		}
	}

	public void SetPlayer(GameObject player)
	{
		playerHealth = player.GetComponent<Health>();
	}
}
