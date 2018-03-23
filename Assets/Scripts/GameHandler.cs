using Assets.Scripts.ShipSatellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public ship ship;
    public int mouse_damage = 1;
    // Use this for initialization
	void Start () {
		
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
    }

    private void check_mine(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<mine_meteor>();
        if (_cmp != null)
        {
            Debug.Log("Click on mine");
            addWorker(ship.try_to_get_satellite(), _cmp.workers, workType.miner, _cmp.gameObject);
        }
    }
    private void check_ship(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<ship>();
        if (_cmp != null)
        {
            Debug.Log("Click on ship");
            addWorker(ship.try_to_get_satellite(), _cmp.workers, workType.defer, _cmp.gameObject);
        }
    }
    private void check_enemy(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<enemy_meteor>();
        if (_cmp != null)
        {
            Debug.Log("Click on enemy");
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
}
