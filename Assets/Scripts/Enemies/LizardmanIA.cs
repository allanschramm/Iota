using UnityEngine;

public class LizardmanIA : EnemyController
{
    void Start(){
        isLookingLeft = true;
    }
    
    protected override void Update(){
        base.Update();
    }

    void FixedUpdate(){
        if(life > 0){
            if(isMoving){
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
                transform.GetComponent<Animator>().SetBool("IsWalk", true);
            }
            else{
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                transform.GetComponent<Animator>().SetBool("IsWalk", false);
            }
        }
    }
}
