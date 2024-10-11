using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public AudioMixerGroup mixer;
    public Slider slide;

    public void Music(float vol) 
    {
        Value(ref vol, slide.value);
        mixer.audioMixer.SetFloat("Music", vol);
        PlayerPrefs.SetFloat("music", slide.value);
        PlayerPrefs.Save();
    } 

    public void Sound(float vol)
    {
        Value(ref vol, slide.value);
        mixer.audioMixer.SetFloat("Sound", vol);
        PlayerPrefs.SetFloat("sound", slide.value);
        PlayerPrefs.Save();
    }

    static public void Value(ref float volume, float val)
    {
        switch (val)
        {
            case 1: volume = 0f; break;
            case 0: volume = -80f; break;
            default: break;
        }
    }
}
