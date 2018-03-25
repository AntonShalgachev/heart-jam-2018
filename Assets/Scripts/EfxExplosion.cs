using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfxExplosion : MonoBehaviour {

    private ParticleSystem particleSystem;

    // Use this for initialization
	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		if(particleSystem.isStopped)
        {
            Destroy(gameObject);
        }
	}
}
