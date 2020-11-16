using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform playerTransform;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip sfxJump;
    public AudioClip sfxAttack;
    public AudioClip sfxHit;
    public AudioClip sfxWeapons;
    public AudioClip[] sfxStep;
    public AudioClip sfxEnemyDead;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySFX(AudioClip sfxClip, float volume){
        sfxSource.PlayOneShot(sfxClip, volume);
    }
}
