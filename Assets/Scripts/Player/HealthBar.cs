using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Sprite[] bar;
    public Image healthBarUI;

    private CharacterController2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find ("Player").GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarUI.sprite = bar[player.life];
    }
}