﻿using Assets.Scripts.ShipSatellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    public ship ship;
    public int mouse_damage = 1;
    public GameObject hud;
    public GameObject explosionPrefub;

    public float gameDistance = 0;
    public float gameSpeed = 10;
    public float gameSpeedDelta = 1;
    public float gameDistanceAddPeriod = 0.1f;

    private GameObject gui_hud;
    private GameObject gui_lose;
    private GameObject gui_start;

    private GameObject gui_hp_bar;
    private GameObject gui_energy_bar;
    private GameObject gui_distance;
    // Use this for initialization
    void Start () {
        if(hud != null)
        {
            gui_hud = hud.transform.GetChild(0).gameObject;
            gui_lose = hud.transform.GetChild(1).gameObject;
            gui_start = hud.transform.GetChild(2).gameObject;
            if(gui_hud != null)
            {
                gui_hp_bar = gui_hud.transform.GetChild(4).gameObject;
                gui_energy_bar = gui_hud.transform.GetChild(5).gameObject;
                gui_distance = gui_hud.transform.GetChild(6).gameObject;
            }
        }
        
        StartCoroutine(StartAddDistance());
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                check_mine(hit.collider.gameObject);
                check_ship(hit.collider.gameObject);
                check_enemy(hit.collider.gameObject);
            }
        }
        if(ship == null)
        {
            if(gui_lose != null)
            {
                gui_lose.SetActive(true);
                var _text = gui_lose.transform.GetChild(2).gameObject.GetComponent<Text>();
                if (_text != null)
                {
                    _text.text = "Passed distance - " + gameDistance.ToString();
                }
            }
        }
        else
        {
            if(gui_hp_bar != null)
            {
                var _img = gui_hp_bar.GetComponent<Image>();
                if (_img != null)
                {
                    _img.fillAmount = ship.getHealthAmount();
                }
            }
        }
    }

    private void check_mine(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<mine_meteor>();
        if (_cmp != null)
        {
            addWorker(ship.try_to_get_satellite(), _cmp.workers, workType.miner, _cmp.gameObject);
        }
    }
    private void check_ship(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<ship>();
        if (_cmp != null)
        {
            addWorker(ship.try_to_get_satellite(), _cmp.workers, workType.defer, _cmp.gameObject);
        }
    }
    private void check_enemy(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<enemy_meteor>();
        if (_cmp != null)
        {
            _cmp.healthHit(mouse_damage);
        }
    }

    private bool addWorker(ship_satellite _sat, List<ship_satellite> _workers, workType _wType, GameObject _owner)
    {
        bool _res = false;
        if (_sat != null && _workers != null)
        {
            _workers.Add(_sat);
            _sat.work = _wType;
            _sat.owner = _owner;
            _res = true;
        }
        return _res;
    }

    private IEnumerator StartAddDistance()
    {
        while (true)
        {
            if (ship != null)
            {
                gameDistance += gameSpeed;
                if (gui_distance != null)
                {
                    var _text = gui_distance.GetComponent<Text>();
                    if (_text != null)
                    {
                        _text.text = "Distance : " + gameDistance.ToString();
                    }
                }
                yield return new WaitForSeconds(gameDistanceAddPeriod);
            }
            else
            {
                break;
            }
        }
    }

    public void btn_restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void btn_start_game()
    {
        if(gui_start != null)
        {
            gui_start.SetActive(false);
            StartCoroutine(StartGameAnim());
        }
    }

    private IEnumerator StartGameAnim()
    {
        while (true)
        {
            if (ship != null)
            {
                float step = 5 * Time.deltaTime;
                ship.transform.position = Vector3.MoveTowards(ship.transform.position, Vector3.zero, step);
                if (Vector2.Distance(Vector3.zero, ship.transform.position) <= 0.1)
                {
                    if (ship != null)
                    {
                        ship.godModeSwitch(false);
                        createExplode();
                        if(gui_hud != null)
                        {
                            gui_hud.SetActive(true);
                        }
                    }
                    break;
                }
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public void createExplode()
    {
        if(explosionPrefub != null)
        {
            var _inst = Instantiate(explosionPrefub);
            _inst.transform.position = ship.transform.position;
        }
    }
}