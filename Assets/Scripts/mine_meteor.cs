using Assets.Scripts.ShipSatellite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine_meteor : MonoBehaviour {


    public List<ship_satellite> workers;

    [SerializeField]
    private RandomHelper.Range mineSpeedRange;
    [SerializeField]
    private RandomHelper.Range mineScaleRange;

    public float MineSpeed { get; private set; }

    public void SetMineMultiplier(float m)
    {
        MineSpeed *= m;
    }

    private void UpdateScale()
    {
        
        var _scale = MineSpeed / ((mineSpeedRange.to - mineSpeedRange.from));
        _scale = mineScaleRange.from + _scale * (mineScaleRange.to - mineScaleRange.from);
        transform.localScale = new Vector3(_scale, _scale);
    }

    // Use this for initialization
    void Awake () {
        MineSpeed = mineSpeedRange.GetRandom();
        UpdateScale();
        workers = new List<ship_satellite>();
    }
	
	// Update is called once per frame
	void Update () {
		
    }
}

