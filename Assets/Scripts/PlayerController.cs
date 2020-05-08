using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public float jumpForce;

    private bool grounded;
    private bool jumping;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sprite;

    public Transform groundCheck;
    public Transform attackRange;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 >> LayerMask.NameToLayer ("Ground"));

        if(Input.GetKeyDown("space")){
            jumping = true;
        }

    }

    void FixedUpdate() {
        float move = Input.GetAxis ("Horizontal") * maxSpeed;

        // anim.SetFloat ("Speed", Mathf.Abs(move));

		rb2d.velocity = new Vector2 (move * maxSpeed, rb2d.velocity.y);

		Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (jumping){
            rb2d.AddForce (new Vector2(0f, jumpForce));
            jumping = false;
        }
    }
}
