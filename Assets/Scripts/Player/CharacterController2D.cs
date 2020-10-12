using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	// **************************** NOVAS VARIAVEIS ****************************

	private GameController _GameController;

    private Rigidbody2D playerRb;
    private Animator playerAnim;

    // Variaveis da animação
    public float speed;
    public float jumpForce;
    public bool isLookingLeft;
	public bool dash = false;



    // Variaveis da UI
    public int life; //Life of the player
	public int keys; //keys counter
    
    // Variaveis do pulo
    public Transform groundCheck;
    private bool isGrounded;
    private bool isAttacking;

    //Variaveis do ataque
    public Weapon weaponEquipped;
    private Attack attack;

	// **************************** ANTIGAS VARIAVEIS ****************************


	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 50f; // Limit fall speed

	public bool canDoubleJump = false; //If player can double jump
	[SerializeField] private float m_DashForce = 25f;
	private bool canDash = true;
	private bool isDashing = false; //If player is dashing
	private bool m_IsWall = false; //If there is a wall in front of the player
	private bool IsFalling = false; //If player is sliding in a wall
	private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
	private float prevVelocityX = 0f;
	private bool canCheck = false; //For check if player is wallsliding

	public GameObject cam;

	public bool invincible = false; //If player can die
	private bool canMove = true; //If player can move
	public bool canAttack = true;

	public Transform attackCheck;

	public GameObject throwableObject;
	public bool isTimeToCheck = false;

	private float jumpWallStartX = 0;
	private float jumpWallDistX = 0; //Distance between player and wall
	private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		playerRb = GetComponent<Rigidbody2D>();
		playerAnim = GetComponent<Animator>();

		_GameController = FindObjectOfType(typeof(GameController)) as GameController;
        _GameController.playerTransform = this.transform;
	}

	void Start() {
		attack = GetComponentInChildren<Attack>();
	}

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

		if(Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}

        if(Input.GetButtonDown("Jump") && isGrounded){
            _GameController.PlaySFX(_GameController.sfxJump, 1f);
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if(Input.GetButtonDown("Fire1") && isAttacking == false){
            playerAnim.SetTrigger("Attack");
            _GameController.PlaySFX(_GameController.sfxAttack, 1f);
            isAttacking = true;
            attack.PlayAnimation(weaponEquipped.animation);
			StartCoroutine(AttackCooldown());
			Debug.Log("Atacando");
        }

		if (Input.GetButton("Fire2"))
		{
			dash = true;
		}

		//Implementar arma a ser arremessada
		if (Input.GetButton("Fire3")){
			// GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
			// Vector2 direction = new Vector2(transform.localScale.x, 0);
			// throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			// throwableWeapon.name = "ThrowableWeapon";
		}

        playerRb.velocity = new Vector2(h * speed, speedY);
        
        playerAnim.SetInteger("h", (int) h);
        playerAnim.SetBool("IsGrounded", isGrounded);
        playerAnim.SetFloat("speedY", speedY);
        playerAnim.SetBool("IsAttacking", isAttacking);

    }

	private void FixedUpdate(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
	}

	void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Damage"){
            Debug.Log("Tomou dano");
        }

        if(col.gameObject.tag == "Collectable"){
            Debug.Log("Coletou item");
            _GameController.PlaySFX(_GameController.sfxWeapons, 1f);
        }
	}

	public void AddWeapon(Weapon weapon){
		canAttack = true;
		weaponEquipped = weapon;
		attack.SetWeapon(weaponEquipped.damage);
	}


	public void DoDashDamage()
	{
		weaponEquipped.damage = Mathf.Abs(weaponEquipped.damage);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy" )
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					weaponEquipped.damage = -weaponEquipped.damage;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", weaponEquipped.damage);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}

	public void ApplyDamage(int damage, Vector3 position) 
	{
		if (!invincible)
		{
			playerAnim.SetBool("Hit", true);
			cam.GetComponent<CameraFollow>().ShakeCamera();
			life -= damage;
			Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f ;
			playerRb.velocity = Vector2.zero;
			playerRb.AddForce(damageDir * 10);
			if (life <= 0)
			{
				StartCoroutine(WaitToDead());
			}
			else
			{
				StartCoroutine(Stun(0.25f));
				StartCoroutine(MakeInvincible(1f));
			}
		}
	}

	void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

	void OnEndAttack(){
        isAttacking = false;
    }
	
    void footStep(){
        _GameController.PlaySFX(_GameController.sfxStep[Random.Range(0, _GameController.sfxStep.Length)], 0.5f);
    }

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.2f);
		if(weaponEquipped != null){
			canAttack = true;
		}
	}

	IEnumerator DashCooldown()
	{
		playerAnim.SetBool("IsDashing", true);
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.1f);
		isDashing = false;
		yield return new WaitForSeconds(0.5f);
		canDash = true;
	}

	IEnumerator Stun(float time) 
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator MakeInvincible(float time) 
	{
		invincible = true;
		yield return new WaitForSeconds(time);
		invincible = false;
	}
	IEnumerator WaitToDead()
	{
		playerAnim.SetBool("IsDead", true);
		canMove = false;
		invincible = true;
		canAttack = false;
		yield return new WaitForSeconds(.2f);
		playerRb.velocity = new Vector2(0, playerRb.velocity.y);
		yield return new WaitForSeconds(.8f);
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
}
