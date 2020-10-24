using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int life;
    protected int damage;
    public float speed;

    protected bool isMoving = false;
    protected bool isLookingLeft;
    protected float WalkDistance;

    public float attackDistance;

    protected Animator anim;
    protected Transform player;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rb2d;


    // \/ **************************************************************** ANTIGAS VARIAVEIS **************************************************************** \/

    // limita a distancia pra andar
    // private float minX;
    // private float maxX;
    // private float destinationX;

 
    private int direction;
    // private bool isFollowing;


    // Attack
    // private bool attacking = false;
    // private bool canAttack = false;
    // private GameObject player;

	

	public bool isInvincible = false;
	// private bool isHitted = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find ("Player").GetComponent<Transform>();

        // player = GameObject.FindWithTag ("Player");

        // minX = transform.position.x - WalkDistance;
        // maxX = transform.position.x + WalkDistance;

        // destinationX = minX;
    }

    protected float PlayerDistance(){
        return Vector2.Distance(player.position, transform.position);
    }

	protected void Flip(){
        isLookingLeft = !isLookingLeft;        
        float x = transform.localScale.x * -1;
        speed *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    protected virtual void Update(){
        float distance = PlayerDistance();

        isMoving = (distance <= attackDistance);

        if(isMoving){
            if((player.position.x > transform.position.x && isLookingLeft) || (player.transform.position.x <  transform.position.x && !isLookingLeft)){
                Flip();
            }
        }
    }

    // \/ **************************************************************** ANTIGO CÓDIGO **************************************************************** \/

    // Update is called once per frame
    // void Update()
    // {
    //     if(canAttack && player !=null){
    //         if(isFollowing){
    //             transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    //         }

    //         if(player.transform.position.x > transform.position.x && transform.localScale.x > 0 || player.transform.position.x < transform.position.x && transform.localScale.x < 0){
    //             Flip();
    //         }

    //         transform.GetComponent<Animator>().SetBool("IsWalk", false);
    //         transform.GetComponent<Animator>().SetTrigger("IsAttack");
    //         attacking = Mathf.Abs (player.transform.position.x - transform.position.x) < 10;
    //         StartCoroutine(AttackDelay(2f));

    //     } else {
    //         Move();
    //     }
    // }

    // void OnBecameVisible(){
    //     isFollowing = true;
    // }

    void FixedUpdate() {
 
    }

    // void Move(){
    //     if(life > 0 && !canAttack){
    //         transform.GetComponent<Animator>().SetBool("IsWalk", true);

    //         Vector2 newPosition = transform.position;
    //         newPosition.x += direction * speed * Time.deltaTime;
    //         transform.position = newPosition;
            
    //         if (destinationX == minX && newPosition.x <= destinationX){
    //             destinationX = maxX;
    //             direction = 1;
    //             Flip();
    //         } else if (destinationX == maxX && newPosition.x >= destinationX) {
    //             destinationX = minX;
    //             direction = -1;
    //             Flip();
    //         }
    //     }
    // }

	public virtual void DamageEnemy(int playerDmg) {
		if (!isInvincible) 
		{
			float direction = playerDmg / Mathf.Abs(playerDmg);
			// playerDmg = Mathf.Abs(playerDmg);

            if (life <= 0) {
                transform.GetComponent<Animator>().SetBool("IsDead", true);
                StartCoroutine(DestroyEnemy());
		    }

			life -= playerDmg;
			rb2d.velocity = Vector2.zero;
			rb2d.AddForce(new Vector2(direction * 50f, 10f));
			StartCoroutine(HitTime());
		}
    }

	// void OnCollisionStay2D(Collision2D collision)
	// {
	// 	if (collision.gameObject.tag == "Player" && life > 0)
	// 	{
	// 		collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(damage, transform.position);
	// 	}
	// }


    // public void AttackPlayer(){
    //     if(isFollowing && life > 0){
    //     attacking = true;
    //     canAttack = true;
    //     }
    // }

    // IEnumerator AttackDelay(float time){
    //     canAttack = false;
    //     yield return new WaitForSeconds(time);
    // }

	IEnumerator HitTime()
	{
		// isHitted = true;
		isInvincible = true;
        transform.GetComponent<Animator>().SetTrigger("Hit");
		yield return new WaitForSeconds(0.1f);
		// isHitted = false;
		isInvincible = false;
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