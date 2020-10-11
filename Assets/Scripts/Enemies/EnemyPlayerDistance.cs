using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDistance : MonoBehaviour
{
    public EnemyController Parent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag ("Player") && Parent.life > 0){
            Parent.AttackPlayer();
        }
    }
}