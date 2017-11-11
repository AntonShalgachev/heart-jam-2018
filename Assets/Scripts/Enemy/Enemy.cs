using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public bool isConvertible;
	public event Action onConverted;

	bool converted = false;

	public void TryConvert()
	{
		if (!isConvertible)
			return;

		Debug.LogFormat("{0} is converted", gameObject.name);

		converted = true;
		OnConverted();
	}

	public void OnConverted()
	{
#warning antonsh temp
		var sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.green;

		if (onConverted != null)
			onConverted();
	}

	public bool IsConverted()
	{
		return converted;
	}
}
