using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPart : MonoBehaviour {

    public int index;
    public Collider2D finderCollider;

    private GameObject player;

    // Use this for initialization
    void Start() {
        GetComponent<SpriteRenderer>().enabled = false;
        var _player = GameObject.FindWithTag("Player");
        if (_player != null)
        {
            player = _player;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (finderCollider != null && player != null)
        {
            if(finderCollider.IsTouching(player.GetComponent<Collider2D>()))
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
	}
}
