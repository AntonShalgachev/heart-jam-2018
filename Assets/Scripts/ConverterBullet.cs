using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverterBullet : MonoBehaviour
{
	private int zombieLayer;
	private int wallLayer;
    public GameObject deathEfx_prefab;

    void Start ()
	{
		zombieLayer = LayerMask.NameToLayer("Zombie");
		wallLayer = LayerMask.NameToLayer("Wall");
	}
	
	void Update ()
	{
		
	}

    void OnCollided(GameObject obj)
    {
        if (obj.layer == zombieLayer || obj.layer == wallLayer)
        {
            var enemy = obj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TryConvert();
            }
            Instantiate(deathEfx_prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollided(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        OnCollided(collision.gameObject);
    }
}
