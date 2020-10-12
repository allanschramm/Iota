using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    private Rigidbody2D playerRb;
    private Animator playerAnim;

    public float speed;
    public float jumpForce;

    public int life; //Life of the player
	public int keys; //keys counter
    
    public bool isLookingLeft;

    public Transform groundCheck;
    private bool isGrounded;

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

        if(Input.GetButtonDown("Fire1") && isGrounded && h == 0){
            playerAnim.SetTrigger("attack");
        }

        playerRb.velocity = new Vector2(h * speed, speedY);
        
        playerAnim.SetInteger("h", (int) h);
        playerAnim.SetBool("isGrounded", isGrounded);
        playerAnim.SetFloat("speedY", speedY);

    }

    void FixedUpdate(){

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);

    }

    void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
