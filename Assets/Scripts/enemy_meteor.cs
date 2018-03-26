using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_meteor : MonoBehaviour {

    public int health = 2;
    public int damage = 2;
    public int cost = 1;
    public GameObject diePrefub;
    public GameObject moneyReceiver;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void healthHit(int _damage, bool _cost)
    {
        health -= _damage;
        if (health <= 0)
        {
            if(_cost && moneyReceiver != null)
            {
                var _money = moneyReceiver.GetComponent<Money>();
                if (_money != null)
                {
                    _money.Gain(cost);
                }
            }
            Destroy(gameObject);
        }
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

