using System.Collections;
using UnityEngine;
using Assets.Scripts.ShipSatellite;
using System.Collections.Generic;

public class ship : MonoBehaviour {

    public float health = 10;
    public int energy = 100;
    public GameObject satellitePrefub;
    public List<ship_satellite> workers;
    public bool godMode = true;
    public int satellites_count = 3;

    private float health_max;
    private List<ship_satellite> satellites;
    // Use this for initialization
    void Start () {
        workers = new List<ship_satellite>();
        satellites = new List<ship_satellite>();
        health_max = health;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public ship_satellite try_to_get_satellite()
    {
        ship_satellite _res = null;

        foreach(ship_satellite s in satellites)
        {
            if(s.work == workType.idle)
            {
                _res = s;
                break;
            }
        }

        return _res;
    }
    public void healthHit(int _damage)
    {
        if (!godMode)
        {
            health -= _damage;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void godModeSwitch(bool _val)
    {
        if (godMode != _val)
        {
            godMode = _val;
            if (_val == false)
            {
                if (satellitePrefub != null)
                {
                    for (var i = 0; i < satellites_count; i++)
                    {
                        ship_satellite _inst = Instantiate(satellitePrefub).GetComponent<ship_satellite>();
                        satellites.Add(_inst);
                        _inst.GetComponent<ship_satellite>().ship = gameObject;
                        _inst.transform.position = transform.position;
                    }
                }
            }
        }
    }

    public float getHealthAmount()
    {
        return health / health_max;
    }
}
