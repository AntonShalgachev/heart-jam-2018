using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public bool isConvertible;
	public bool canBeAgressive;

	public event Action onConverted;

	bool converted = false;
	bool agressive = false;

	public void TryConvert()
	{
		if (!isConvertible || agressive)
			return;

		Debug.LogFormat("{0} is converted", gameObject.name);

		converted = true;
		OnConverted();
	}

	void OnConverted()
	{
#warning antonsh temp
		var sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.green;

		if (onConverted != null)
			onConverted();
	}

	public void TryMakeAgressive()
	{
		if (!canBeAgressive || converted)
			return;

		Debug.LogFormat("{0} became agressive", gameObject.name);

		agressive = true;
		OnMadeAgressive();
	}

	void OnMadeAgressive()
	{
#warning antonsh temp
		var sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.red;
	}

	public bool IsConverted()
	{
		return converted;
	}

	public bool IsAgressive()
	{
		return agressive;
	}
}
