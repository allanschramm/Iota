using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    private Rigidbody2D playerRb;
    private Animator playerAnim;

    // Variaveis da animação
    public float speed;
    public float jumpForce;
    public bool isLookingLeft;


    // Variaveis da UI
    public int life; //Life of the player
	public int keys; //keys counter
    
    // Variaveis do pulo
    public Transform groundCheck;
    private bool isGrounded;
    private bool isAttacking;

    //Variaveis do ataque
    public Transform attack;
    public GameObject hitBoxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal") * speed;

        if(isAttacking && isGrounded){
            h = 0;
        }

        if(h > 0 && isLookingLeft){
            Flip();
        }
        else if (h < 0 && !isLookingLeft){
            Flip();
        }

        float speedY = playerRb.velocity.y;

        if(Input.GetButtonDown("Jump") && isGrounded){
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if(Input.GetButtonDown("Fire1") && isAttacking == false){
            isAttacking = true;
            playerAnim.SetTrigger("Attack");
        }

        playerRb.velocity = new Vector2(h * speed, speedY);
        
        playerAnim.SetInteger("h", (int) h);
        playerAnim.SetBool("IsGrounded", isGrounded);
        playerAnim.SetFloat("speedY", speedY);
        playerAnim.SetBool("IsAttacking", isAttacking);

    }

    void FixedUpdate(){

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);

    }

    void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void OnEndAttack(){
        isAttacking = false;
    }

    void hitBoxAttack(){
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, attack.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }
}
