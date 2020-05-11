using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float WalkDistance;
    private float minX;
    private float maxX;

    private float destinationX;
    private Rigidbody2D rb2d;

    private int direction = -1;
    public int speed;
    float health = 12;

    private bool attacking = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        minX = transform.position.x - WalkDistance;
        maxX = transform.position.x + WalkDistance;

        destinationX = minX;
        rb2d = GetComponent<Rigidbody2D> ();

        player = GameObject.FindWithTag ("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking && player !=null) {
            if(player.transform.position.x > transform.position.x && transform.localScale.x > 0
            || player.transform.position.x <  transform.position.x && transform.localScale.x < 0){
                Flip();
            }

            if(true){

            }

            attacking = Mathf.Abs (player.transform.position.x - transform.position.x) < 10;

        } else {
            Move();
        }
    }

    void Move(){
        Vector2 newPosition = transform.position;
        newPosition.x += direction * speed * Time.deltaTime;
        transform.position = newPosition;
        
        if (destinationX == minX && newPosition.x <= destinationX){
            destinationX = maxX;
            direction = 1;
            Flip();
        } else if (destinationX == maxX && newPosition.x >= destinationX) {
            destinationX = minX;
            direction = -1;
            Flip();
        }
    }

    //  public void Damage(){
    //     health--;
    //     if(health <= 0){
    //         Destroy (gameObject);
    //     }
    // }

    public void ApplyDamage(float damage) {
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			health -= damage;
			rb2d.velocity = Vector2.zero;
			rb2d.AddForce(new Vector2(direction * 500f, 100f));
			// StartCoroutine(HitTime());
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && health > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
		}
	}

    void Flip(){
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void AttackPlayer(){
        attacking = true;
    }

    IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}
}