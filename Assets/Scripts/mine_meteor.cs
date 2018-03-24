using Assets.Scripts.ShipSatellite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine_meteor : MonoBehaviour {


    public List<ship_satellite> workers;

    [SerializeField]
    private RandomHelper.Range mineSpeedRange;

    public float MineSpeed { get; private set; }

    // Use this for initialization
    void Start () {
        MineSpeed = mineSpeedRange.GetRandom();

        workers = new List<ship_satellite>();

        var _scale = MineSpeed / ((mineSpeedRange.to - mineSpeedRange.from));
        _scale *= transform.localScale.x;

        transform.localScale = new Vector3(_scale, _scale);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
