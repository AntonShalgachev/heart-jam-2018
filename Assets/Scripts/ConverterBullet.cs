using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverterBullet : MonoBehaviour
{
	private int zombieLayer;
	private int wallLayer;

	void Start ()
	{
		zombieLayer = LayerMask.NameToLayer("Zombie");
		wallLayer = LayerMask.NameToLayer("Wall");
	}
	
	void Update ()
	{
		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var obj = collision.gameObject;
		if (obj.layer == zombieLayer || obj.layer == wallLayer)
		{
			var enemy = obj.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.TryConvert();
			}

			Destroy(gameObject);
		}
	}
}
