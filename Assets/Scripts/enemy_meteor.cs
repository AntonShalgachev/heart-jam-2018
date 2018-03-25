﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_meteor : MonoBehaviour {

    public int health = 2;
    public int damage = 2;
    public GameObject diePrefub;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void healthHit(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var _ship = col.gameObject.GetComponent<Ship>();
        
        if (_ship != null)
        {
            _ship.healthHit(damage);
            Destroy(gameObject);
        }
    }
}

