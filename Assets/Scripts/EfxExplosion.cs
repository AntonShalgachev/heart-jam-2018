using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfxExplosion : MonoBehaviour {

    private ParticleSystem partleSystem;

    // Use this for initialization
	void Start () {
        partleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		if(partleSystem.isStopped)
        {
            Destroy(gameObject);
        }
	}
}
