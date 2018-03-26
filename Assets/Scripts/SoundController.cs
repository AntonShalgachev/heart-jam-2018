using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour {

    public List<AudioSource> sources;
    private static bool soundState = false;
    public Image image;
    public Sprite sndOff;
    public Sprite sndOn;
    // Use this for initialization
    void Start () {
        string _str = PlayerPrefs.GetString("soundSwitch", "False");
        soundState = Convert.ToBoolean(_str);
        if(soundState)
        {
            stopAll();
            setSprite(sndOff);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void stopAll()
    {
        foreach (AudioSource _src in sources)
        {
            _src.mute = soundState;
        }
    }
    private void setSprite(Sprite _spr)
    {
        if (image != null)
        {
            image.sprite = _spr;
        }
    }
    public void switchSound()
    {
        soundState = !soundState;
        stopAll();
        if (!soundState)
        {
            setSprite(sndOn);
        }
        else
        {
            setSprite(sndOff);
        }
        PlayerPrefs.SetString("soundSwitch", soundState.ToString());
    }

    public static bool getSoundState()
    {
        return soundState;
    }
    public static void playSound(AudioSource _src, AudioClip _clip)
    {
        if(_src != null && !soundState && _clip != null)
        {
            _src.clip = _clip;
            _src.Play();
        }
    }
}
