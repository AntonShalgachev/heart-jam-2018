using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHelper : MonoBehaviour {

    public List<Sprite> skins;
    public float rotationSpeed = 0;
    // Use this for initialization
	void Start () {
        var _renderer = GetComponent<SpriteRenderer>();
        var random = RandomHelper.Instance();
        if (_renderer != null && skins.Count > 0)
        {
            _renderer.sprite = skins[random.GetInt(0, skins.Count)];
        }
        transform.rotation = Quaternion.Euler(0, 0, random.GetInt(0, 360));
        if (rotationSpeed > 0)
        {
            rotationSpeed = random.GetFloat(-rotationSpeed, rotationSpeed);
        }

    }
	
	// Update is called once per frame
	void Update () {
		if(rotationSpeed > 0)
        {
            var _rot = transform.rotation.eulerAngles;
            _rot.z += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(_rot);
        }
	}
}
