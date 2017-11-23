using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMonitor : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Tilemap tilemap = GetComponentInParent<Tilemap>();
		if(tilemap != null)
        {
            EditorTile tile = tilemap.GetTile<EditorTile>(Vector3Int.RoundToInt(transform.position));
            if (tile != null)
            {
                transform.rotation = Quaternion.Euler(0, 0, tile.m_Rot);
                transform.position += getShift(tile.m_Rot);
                //print("rot = " + tile.m_Rot.ToString());
            }
        }
        //print("Start");
    }
	private Vector3 getShift(float _rot)
    {
        Vector3 _res = Vector3.zero;

        if (_rot == 90f)
        {
            _res = new Vector3(1f,0f,0f);
        }
        if (_rot == 180f)
        {
            _res = new Vector3(1f, 1f, 0f);
        }
        if (_rot == 270f)
        {
            _res = new Vector3(0f, 1f, 0f);
        }
        return _res;
    }
}
