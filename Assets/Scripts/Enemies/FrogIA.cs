using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogIA : EnemyController
{
    void Start(){
        life = 8;
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
        else{
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            transform.GetComponent<Animator>().SetBool("IsWalk", false);
        }
        if (life <= 0) {
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}
        
    }

    IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
