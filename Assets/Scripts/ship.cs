using System.Collections;
using UnityEngine;
using Assets.Scripts.ShipSatellite;
using System.Collections.Generic;

public class ship : MonoBehaviour {

    public int health = 10;
    public int energy = 100;
    public GameObject satellitePrefub;
    private List<ship_satellite> satellites;
    public List<ship_satellite> workers;
    // Use this for initialization
    void Start () {
        workers = new List<ship_satellite>();
        satellites = new List<ship_satellite>();
        if (satellitePrefub != null)
        {
            ship_satellite _inst = Instantiate(satellitePrefub).GetComponent<ship_satellite>();
            satellites.Add(_inst);
            _inst.GetComponent<ship_satellite>().ship = gameObject;
            _inst.transform.position = transform.position;
        }
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
        health -= _damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            // lose game
        }
    }
}
