using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour {

    public List<AudioSource> sources;
    private bool soundState = false;
    public Image image;
    public Sprite sndOff;
    public Sprite sndOn;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void switchSound()
    {
        soundState = !soundState;
        foreach (AudioSource _src in sources)
        {
            _src.mute = soundState;
        }
        if(image != null)
        {
            if(!soundState)
            {
                image.sprite = sndOn;
            }
            else
            {
                image.sprite = sndOff;
            }
        }
    }
}
