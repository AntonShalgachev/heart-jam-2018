using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject deathEfx_prefab;

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

    void OnCollided(GameObject obj)
    {
        if (obj.layer == zombieLayer || obj.layer == wallLayer)
        {
            Instantiate(deathEfx_prefab, transform.position, Quaternion.identity);
            print("efx");
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
