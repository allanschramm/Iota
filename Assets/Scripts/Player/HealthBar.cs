﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Sprite[] bar;
    public Image healthBarUI;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find ("Alien").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarUI.sprite = bar[player.health];
    }
}