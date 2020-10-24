using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardmanIA : EnemyController
{
    void Start(){
        life = 80;
        isLookingLeft = true;
    }
    
    protected override void Update(){
        base.Update();
    }

    void FixedUpdate(){
        if(isMoving){
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            transform.GetComponent<Animator>().SetBool("IsWalk", true);

        }
    }
}
