using UnityEngine;

public class FrogIA : EnemyController
{
    void Start(){
        isLookingLeft = true;
    }

    protected override void Update(){
        base.Update();
    }

    void FixedUpdate(){
        // Cotrola animação de andar até o player
        if(life > 0){
            if(isMoving && !isAttacking){
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
                transform.GetComponent<Animator>().SetBool("IsAttacking", false);
                transform.GetComponent<Animator>().SetBool("IsWalk", true);
            }

            // Controla animação de Idle
            if(!isMoving && !isAttacking){
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                transform.GetComponent<Animator>().SetBool("IsWalk", false);
                transform.GetComponent<Animator>().SetBool("IsAttacking", false);
            }
            
            // Controla animação de Attack
            if(!isMoving && isAttacking){
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                transform.GetComponent<Animator>().SetBool("IsWalk", false);
                transform.GetComponent<Animator>().SetBool("IsAttacking", true);
            }
        }
    }
}
