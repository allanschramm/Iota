using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform playerTransform;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public OptionsMenu somMenu;
    private float volume;

    public AudioClip sfxJump;
    public AudioClip sfxAttack;
    public AudioClip sfxHit;
    public AudioClip sfxWeapons;
    public AudioClip[] sfxStep;
    public AudioClip sfxEnemyDead;

    // Start is called before the first frame update
    void Start()
    {
        volume = somMenu.GetVolume();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySFX(AudioClip sfxClip, float volumeF){
        volumeF = volume;
        sfxSource.PlayOneShot(sfxClip, volumeF);
    }
}
