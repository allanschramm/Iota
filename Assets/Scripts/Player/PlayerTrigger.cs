﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{

    private CharacterController2D player;
    private EnemyController enemy;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find ("Player").GetComponent<CharacterController2D>();
    }

    void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Enemy")){
			EnemyController enemy = other.GetComponent<EnemyController> ();

            if(!player.invincible){
                player.ApplyDamage(enemy.damage);
            }
        }
	}
}
