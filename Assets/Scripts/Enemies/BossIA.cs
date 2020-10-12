using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIA : MonoBehaviour
{
    public float life;
    public int damage;

    // limita a distancia pra andar
    public float WalkDistance;
    private float minX;
    private float maxX;
    private float destinationX;
    
    private Rigidbody2D rb2d;
    private int direction = -1;
    public int speed;

    private bool attacking = false;
    private GameObject player;

	

	public bool isInvincible = false;
	// private bool isHitted = false;

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

            attacking = Mathf.Abs (player.transform.position.x - transform.position.x) < 10;

        } else {
            Move();
        }
    }

    void FixedUpdate() {
        if (life <= 0) {
			transform.GetComponent<Animator>().SetTrigger("IsDead");
			StartCoroutine(DestroyEnemy());
		}
    }

    void Move(){
        if(life > 0){
            transform.GetComponent<Animator>().SetTrigger("Walk");

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
    }

	public void ApplyDamage(float damage) {
		if (!isInvincible) 
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			life -= damage;
			rb2d.velocity = Vector2.zero;
			rb2d.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
            Debug.Log("Tomou hit");
		}
    }

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(damage
    , transform.position);
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

	IEnumerator HitTime()
	{
		// isHitted = true;
		isInvincible = true;
        transform.GetComponent<Animator>().SetTrigger("Hit");
		yield return new WaitForSeconds(0.2f);
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
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
