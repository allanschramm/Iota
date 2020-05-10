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

    // Weapon System
    public Transform attackRange;
    public WeaponController[] AllWeapons;
    List<WeaponController> EnabledWeapons = new List<WeaponController> ();
    WeaponController CurrentWeapon = null;
    int weaponIndex = -1;

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
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if(Input.GetButtonDown("Jump") && grounded){
            jumping = true;
        }

        if (Input.GetButtonDown("Fire1")){
            DoAttack();
        }

        if (Input.GetKeyDown (KeyCode.Q)){
            ChangeWeapon();
        }
    }

    void FixedUpdate() {
        float move = Input.GetAxis ("Horizontal") * maxSpeed;
        move = Mathf.Clamp(move, -maxSpeed, maxSpeed);
		rb2d.velocity = new Vector2 (move * maxSpeed, rb2d.velocity.y);

        // anim.SetFloat ("Speed", Mathf.Abs(move));

		Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (jumping){
            rb2d.AddForce (new Vector2(0f, jumpForce));
            jumping = false;
        }

        // Troca de animações
		if (move > 0 && sprite.flipX || move < 0 && !sprite.flipX) {
			Flip ();
		}

        
    }

    void Flip()
	{
		sprite.flipX = !sprite.flipX;
	}

    public void EnableWeapon(int index){
        if(index >= 0 && 0 < AllWeapons.Length){
            if(!EnabledWeapons.Contains(AllWeapons[index])){
                EnabledWeapons.Add (AllWeapons [index]);
            }
        }
    }

    void ChangeWeapon(){
        if(CurrentWeapon !=null)
            CurrentWeapon.gameObject.SetActive (false);

        weaponIndex++;

        if (weaponIndex == EnabledWeapons.Count){
            weaponIndex = -1;
        } else {
            CurrentWeapon = EnabledWeapons [weaponIndex];
            CurrentWeapon.gameObject.SetActive (true);
        }
    }

    void DoAttack(){
        if (CurrentWeapon != null){
            CurrentWeapon.Attack();
        }
    }

       
}

public class WeaponGrab : MonoBehaviour{
    public int WeaponNumber;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag ("Player")) {
            other.GetComponent<PlayerController> ().EnableWeapon (WeaponNumber);
            Destroy (gameObject);
        }
    }
}