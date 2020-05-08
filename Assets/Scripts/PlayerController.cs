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
    public Transform AttackRange;

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

        if(Input.GetButtonDown("Jump") && grounded){
            jumping = true;
        }

    }

    void FixedUpdate() {
        if (jumping){
            rb2d.AddForce (new Vector2(0f, jumpForce));
        }
    }
}
