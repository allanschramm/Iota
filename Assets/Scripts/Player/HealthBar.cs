using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Sprite[] bar;
    public Image healthBarUI;

    private CharacterController2D player;
    private CharacterController2D playerNew;


    // Start is called before the first frame update
    // void Start()
    // {
    //     player = GameObject.Find ("Player").GetComponent<CharacterController2D>();
    //     playerNew = GameObject.Find ("PlayerNew").GetComponent<CharacterController2D>();

    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     healthBarUI.sprite = bar[player.life];
    //     healthBarUI.sprite = bar[playerNew.life];
    // }
}