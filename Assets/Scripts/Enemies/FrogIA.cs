using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogIA : MonoBehaviour
{
    public float life;
    public int dmg;
    public bool isInvincible = false;
    public PlayerController2D player;

    private Rigidbody2D frogRb;
    private Animator frogAnim;

    public float speed;
    public float timeToWalk;

    public GameObject HitBox;
    public bool isLookingLeft;

    private int h;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController2D>();

        frogRb = GetComponent<Rigidbody2D>();
        frogAnim = GetComponent<Animator>();

        StartCoroutine("frogWalk");

    }

    // Update is called once per frame
    void Update()
    {

        if(h > 0 && isLookingLeft){
            Flip();
        }
        else if (h < 0 && !isLookingLeft){
            Flip();
        }
        
        frogRb.velocity = new Vector2(h * speed, frogRb.velocity.y);

        if(h != 0){
            frogAnim.SetBool("IsWalk", true);
        }
        else{
            frogAnim.SetBool("IsWalk", false);
        }

    }

    void FixedUpdate() {
        if (life <= 0) {
			transform.GetComponent<Animator>().SetTrigger("IsDead");
			StartCoroutine(DestroyEnemy());
		}
    }

	public void ApplyDamage(int damage) {
		if (!isInvincible) 
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			life -= damage;
			frogRb.velocity = Vector2.zero;
			frogRb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
            Debug.Log("Sapo tomou dano!");
		}
    }

    void OnCollisionStay2D(Collision2D collision){
		if (collision.gameObject.tag == "Player" && life > 0)
		{
            h = 0;
            StopCoroutine("frogWalk");
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(dmg, transform.position);
		}
	}

    void OnDead(){
        Destroy(this.gameObject);
    }

    void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

    }

    IEnumerator frogWalk(){
        int rand = Random.Range(0, 100);

        if(rand < 33){
            h = -1;
        }
        else if(rand < 66){
            h = 0;
        }
        else{
            h = 1;
        }

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("frogWalk");
    }

    IEnumerator HitTime(){
		// isHitted = true;
		isInvincible = true;
        transform.GetComponent<Animator>().SetTrigger("Hit");
		yield return new WaitForSeconds(0.2f);
		// isHitted = false;
		isInvincible = false;
	}

    IEnumerator DestroyEnemy(){
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		frogRb.velocity = new Vector2(0, frogRb.velocity.y);
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
