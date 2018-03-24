using System.Collections;
using UnityEngine;
using Assets.Scripts.ShipSatellite;
using System.Collections.Generic;

public class ship : MonoBehaviour {

    public float health = 10;
    public float energy = 100;
    public float energySpeed = 0.1f;
    public GameObject satellitePrefub;
    public List<ship_satellite> workers;
    public bool godMode = true;
    public int satellites_count = 3;

    private float health_max;
    private float energy_max;

    private List<ship_satellite> satellites;
    private GameObject main_engine;

    private bool engine_anim_dir = true;
    private SpriteRenderer spriteRenderer;
    private int engineStrength = 2;
    // Use this for initialization
    void Start () {
        workers = new List<ship_satellite>();
        satellites = new List<ship_satellite>();
        health_max = health;
        energy_max = energy;
        main_engine = transform.GetChild(2).gameObject;
        spriteRenderer = main_engine.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        engineAnim(engineStrength);
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
                engineStrength = 1;
                StartCoroutine(StartLoseEnergy());
            }
        }
    }

    public float getHealthAmount()
    {
        return health / health_max;
    }

    public float getEnergyAmount()
    {
        return energy / energy_max;
    }

    private void engineAnim(int strength)
    {
        float alpha_speed = 1f;
        float alpha_min = 0.90f;
        float alpha_max = 1f;

        switch(strength)
        {
            case 0:
                alpha_min = 0.7f;
                alpha_max = 0.90f;
                alpha_speed = 1.5f;
                break;
            case 2:
                alpha_min = 1.2f;
                alpha_max = 1.3f;
                alpha_speed = 2f;
                break;
        }

        if (main_engine != null && spriteRenderer != null)
        {
            if(engine_anim_dir)
            {
                var _col = spriteRenderer.color;
                if (twinkValue(out _col.a, _col.a, alpha_max, alpha_speed * Time.deltaTime))
                {
                    engine_anim_dir = false;
                }
                main_engine.transform.localScale = new Vector3(_col.a, _col.a);
                spriteRenderer.color = _col;
            }
            else
            {
                var _col = spriteRenderer.color;
                if (twinkValue(out _col.a, _col.a, alpha_min, -alpha_speed * Time.deltaTime))
                {
                    engine_anim_dir = true;
                }
                main_engine.transform.localScale = new Vector3(_col.a, _col.a);
                spriteRenderer.color = _col;
            }
        }
    }
    bool twinkValue(out float _val, float from, float to, float delta)
    {
        bool _res = false;
        _val = from + delta;
        if (delta > 0)
        {
            if (_val >= to)
            {
                _res = true;
            }
        }
        else
        {
            if (_val <= to)
            {
                _res = true;
            }
        }
        return _res;
    }

    private IEnumerator StartLoseEnergy()
    {
        while (true)
        {
            energy -= energySpeed;
            if (energy < 0) energy = 0;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
