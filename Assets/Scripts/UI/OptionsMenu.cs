using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    private float masterVolume;
    
    public void SetVolume(float volume){
        masterVolume = volume;
        audioMixer.SetFloat("MainVolume", volume);
    }

    public float GetVolume(){
        return masterVolume;
    }

}
