using Assets.Scripts.ShipSatellite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine_meteor : MonoBehaviour {


    public List<ship_satellite> workers;
    [Serializable]
    public class Mine_params
    {
        public float mineSpeed = 0.5f;
        public float mineSpeedMax = 1f;
        public float mineSpeedMin = 0.05f;
    }

    public Mine_params mine_params;

    // Use this for initialization
    void Start () {
        workers = new List<ship_satellite>();
        var _scale = mine_params.mineSpeed/((mine_params.mineSpeedMax - mine_params.mineSpeedMin) / 2);

        transform.localScale = new Vector3(_scale, _scale);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
