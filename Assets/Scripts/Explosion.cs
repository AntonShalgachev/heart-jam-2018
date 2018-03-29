using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float scale_speed = 1;
    public AudioClip sndExplosion;
    private bool isExploded = false;
    private GameObject efx;
    // Use this for initialization
	void Start () {
        efx = transform.GetChild(0).gameObject;
        SoundController.playSound(GetComponent<AudioSource>(), sndExplosion);
    }
	
	// Update is called once per frame
	void Update () {
        float step = scale_speed * Time.deltaTime;
        if (!isExploded)
        {
            if (transform.localScale.x < 1)
            {
                Vector3 _tmp = new Vector3(Mathf.Lerp(transform.localScale.x, 1.1f, 10 * step), Mathf.Lerp(transform.localScale.x, 1.1f, 10 * step));
                transform.localScale = _tmp;
            }
            else
            {
                isExploded = true;
            }
        }
        else
        {
            Vector3 _tmp = new Vector3(Mathf.Lerp(transform.localScale.x, 0, 3 * step), Mathf.Lerp(transform.localScale.x, 1, 3 * step));
            transform.localScale = _tmp;
            if (transform.localScale.x < 0.1)
            {
                if(efx == null)
                {
                    Destroy(gameObject);
                }
            }
        }
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        destoyType(col, "enemy_meteor");
        destoyType(col, "mine_meteor");
    }

    void destoyType(Collision2D _col, string _type)
    {
        var _cmp = _col.gameObject.GetComponent(_type);

        if (_cmp != null)
        {
            Destroy(_cmp.gameObject);
        }
    }
}
