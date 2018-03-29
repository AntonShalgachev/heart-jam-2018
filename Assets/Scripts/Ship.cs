﻿using System.Collections;
using UnityEngine;
using Assets.Scripts.ShipSatellite;
using System.Collections.Generic;
using System;

public class Ship : MonoBehaviour {

    public float health = 10;
    public float energy = 100;
    public float energySpeed = 0.1f;
    public GameObject satellitePrefub;
    public List<ship_satellite> workers;
    public bool godMode = true;
    public int satellites_count = 3;

    public GameObject shield;

    public float energyToMoneyConversionRatio;
    public int moneyPerDestroyedMeteor;

    public float hpPurchaseAmount;
    public int hpPurchasePrice;
    public float shieldPurchaseDuration;
    public int shieldPurchasePrice;
    public int satellitePurchasePrice;

    private float health_max;
    private float energy_max;

    private List<ship_satellite> satellites;
    private GameObject main_engine;
    private GameObject fuel;

    private bool engine_anim_dir = true;
    private SpriteRenderer spriteRenderer;
    private int engineStrength = 2;

    private bool shieldActive;

    private Money currencyManager;
    private int hpAddCount = 1;
    private int shieldCount = 1;
    public event Action<int> OnCurrencyChanged;

    // Use this for initialization
    void Awake () {
        workers = new List<ship_satellite>();
        satellites = new List<ship_satellite>();
        health_max = health;
        energy_max = energy;
        main_engine = transform.GetChild(2).gameObject;
        fuel = transform.GetChild(1).gameObject;
        spriteRenderer = main_engine.GetComponent<SpriteRenderer>();

        currencyManager = GetComponent<Money>();
        currencyManager.OnCurrencyChanged += OnMoneyChanged;
        Debug.Assert(currencyManager);

        SetShieldEnabled(false);
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
            if (s != null)
            {
                if (s.work == workType.idle)
                {
                    _res = s;
                    break;
                }
            }
        }

        return _res;
    }
    public void healthHit(int _damage)
    {
        if (!godMode && !shieldActive)
        {
            health -= _damage;
            if (health <= 0)
            {
                wrapDestroy();
            }
        }
    }

    private void DisableShield()
    {
        SetShieldEnabled(false);
    }

    private void SetShieldEnabled(bool enabled)
    {
        shield.SetActive(enabled);
        shieldActive = enabled;
    }

    private void AddSatellite()
    {
        ship_satellite _inst = Instantiate(satellitePrefub).GetComponent<ship_satellite>();
        satellites.Add(_inst);
        _inst.GetComponent<ship_satellite>().ship = gameObject;

        var pos = transform.position;
        pos.z = satellitePrefub.transform.position.z;
        _inst.transform.position = pos;
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
                        AddSatellite();
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
        if(main_engine != null)
        {
            engine_anim_dir = engineAnim_ext(strength, spriteRenderer, main_engine.transform, engine_anim_dir);
        }
    }
    public static bool engineAnim_ext(int strength, SpriteRenderer _render, Transform _trans, bool _dir)
    {
        float alpha_speed = 1f;
        float alpha_min = 0.90f;
        float alpha_max = 1f;
        bool _res = _dir;

        switch (strength)
        {
            case 0:
                alpha_min = 0.17f;
                alpha_max = 0.22f;
                alpha_speed = 0.5f;
                break;
            case 2:
                alpha_min = 1.2f;
                alpha_max = 1.3f;
                alpha_speed = 2f;
                break;
        }


        if (_dir)
        {
            var _col = _render.color;
            if (twinkValue(out _col.a, _col.a, alpha_max, alpha_speed * Time.deltaTime))
            {
                _res = false;
            }
            _trans.localScale = new Vector3(_col.a, _col.a);
            _render.color = _col;
        }
        else
        {
            var _col = _render.color;
            if (twinkValue(out _col.a, _col.a, alpha_min, -alpha_speed * Time.deltaTime))
            {
                _res = true;
            }
            _trans.localScale = new Vector3(_col.a, _col.a);
            _render.color = _col;
        }
        return _res;
    }

    static bool twinkValue(out float _val, float from, float to, float delta)
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
            var tutorial = TutorialController.Instance;
            if (!tutorial.IsEnabled() || !tutorial.IsStepCompleted(TutorialController.Step.InitialDelay))
            {
                energy -= energySpeed;
                if (fuel != null)
                {
                    float y_scale = 0.1f + 1.6f * (energy / energy_max);
                    fuel.transform.localScale = new Vector3(1f, y_scale);
                }
                if (energy < 0)
                {
                    energy = 0;
                    wrapDestroy();
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void wrapDestroy()
    {
        Destroy(gameObject);
    }

    public void addEnergy(float amount)
    {
        energy += amount;
        if(energy > energy_max)
        {
            var excess = energy - energy_max;
            currencyManager.Gain(excess * energyToMoneyConversionRatio);
            energy = energy_max;
        }
    }

    public void OnMeteorDestroyedByMouse()
    {
        currencyManager.Gain(moneyPerDestroyedMeteor);
    }

    private void OnMoneyChanged(int value)
    {
        if (OnCurrencyChanged != null)
            OnCurrencyChanged(value);
    }

    public void OnPurchaseShield()
    {
        if (godMode)
            return;

        if (currencyManager.TryPurchase(shieldPurchasePrice * shieldCount))
        {
            SetShieldEnabled(true);
            shieldCount++;
            Invoke("DisableShield", shieldPurchaseDuration);
        }
    }

    public void OnPurchaseHP()
    {
        if (godMode)
            return;

        if (currencyManager.TryPurchase(hpPurchasePrice * hpAddCount))
        {
            health = Mathf.Clamp(health + hpPurchaseAmount, 0, health_max);
            hpAddCount++;
        }
    }

    public void OnPurchaseSatellite()
    {
        if (godMode)
            return;

        if (currencyManager.TryPurchase(satellitePurchasePrice * (1 + satellites.Count)))
        {
            AddSatellite();
        }
    }
    public enum Items { Shield, HP, Satellite};

    public int getPrice(Items _type)
    {
        int _res = -1;
        switch (_type)
        {
            case Items.Shield:
                _res = shieldPurchasePrice * shieldCount;
                break;
            case Items.HP:
                _res = hpPurchasePrice * hpAddCount;
                break;
            case Items.Satellite:
                satellites.RemoveAll(item => item == null);
                _res = satellitePurchasePrice * (1 + satellites.Count);
                break;
        }
        return _res;
    }
}
