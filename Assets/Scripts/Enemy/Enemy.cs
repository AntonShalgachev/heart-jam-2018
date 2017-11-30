using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public bool isConvertible;
	public bool canBeAgressive;

    public Color cInert;
    public Color cAgressive;
    public Color cAlly;

    public event Action onConverted;

	bool converted = false;
	bool agressive = false;

    private void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = cInert;
    }

    public void TryConvert()
	{
		if (!isConvertible || agressive)
			return;

		converted = true;
		OnConverted();
	}

	void OnConverted()
	{
		var sprite = GetComponent<SpriteRenderer>();
		sprite.color = cAlly;

		if (onConverted != null)
			onConverted();
	}

	public void TryMakeAgressive()
	{
		if (!canBeAgressive || converted)
			return;

		agressive = true;
		OnMadeAgressive();
	}

	void OnMadeAgressive()
	{
		var sprite = GetComponent<SpriteRenderer>();
		sprite.color = cAgressive;
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
