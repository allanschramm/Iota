using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_WallCheck;								//Posicion que controla si el personaje toca una pared

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D playerRb;
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

	private GameController _GameController;

	public int life; //Life of the player
	public int keys; //keys counter

	public GameObject cam;

	public bool invincible = false; //If player can die
	private bool canMove = true; //If player can move
	public bool canAttack = true;

	private Animator playerAnim;

	public Transform attackCheck;

	public GameObject throwableObject;

	public Weapon weaponEquipped;
	private Attack attack;
	public bool isTimeToCheck = false;

	private float jumpWallStartX = 0;
	private float jumpWallDistX = 0; //Distance between player and wall
	private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps

	[Header("Events")]
	[Space]

	public UnityEvent OnFallEvent;
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		playerRb = GetComponent<Rigidbody2D>();
		playerAnim = GetComponent<Animator>();

		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
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

        if(Input.GetButtonDown("Jump") && isGrounded){
            _GameController.PlaySFX(_GameController.sfxJump, 1f);
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if(Input.GetButtonDown("Fire1") && isAttacking == false){
            _GameController.PlaySFX(_GameController.sfxAttack, 1f);
            isAttacking = true;
            playerAnim.SetTrigger("Attack");
            attack.PlayAnimation(weaponEquipped.animation);
        }

        playerRb.velocity = new Vector2(h * speed, speedY);
        
        playerAnim.SetInteger("h", (int) h);
        playerAnim.SetBool("IsGrounded", isGrounded);
        playerAnim.SetFloat("speedY", speedY);
        playerAnim.SetBool("IsAttacking", isAttacking);

    }

	private void FixedUpdate()
	{
		if (Input.GetButton("Vertical"))
		{
			// Change Weapon

		}

		if(Input.GetKeyDown(KeyCode.R))
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

		if (Input.GetButton("Fire1") && canAttack)
		{
			playerAnim.SetBool("IsAttacking", true);
			attack.PlayAnimation(weaponEquipped.animation);
			canAttack = false;
			StartCoroutine(AttackCooldown());
			Debug.Log("Atacando");
		}

		//Implementar arma a ser arremessada
		if (Input.GetButton("Fire3"))
		{
			// GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
			// Vector2 direction = new Vector2(transform.localScale.x, 0);
			// throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			// throwableWeapon.name = "ThrowableWeapon";
		}

		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
				if (!wasGrounded )
				{
					OnLandEvent.Invoke();

					canDoubleJump = true;
					if (playerRb.velocity.y < 0f)
						limitVelOnWallJump = false;
				}
		}

		m_IsWall = false;

		if (!m_Grounded)
		{
			OnFallEvent.Invoke();
			Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != null)
				{
					isDashing = false;
					m_IsWall = true;
				}
			}
			prevVelocityX = playerRb.velocity.x;
		}

		if (limitVelOnWallJump)
		{
			if (playerRb.velocity.y < -0.5f)
				limitVelOnWallJump = false;
			jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
			if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
			{
				canMove = true;
			}
			else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
			{
				canMove = true;
				playerRb.velocity = new Vector2(10f * transform.localScale.x, playerRb.velocity.y);
			}
			else if (jumpWallDistX < -2f) 
			{
				limitVelOnWallJump = false;
				playerRb.velocity = new Vector2(0, playerRb.velocity.y);
			}
			else if (jumpWallDistX > 0) 
			{
				limitVelOnWallJump = false;
				playerRb.velocity = new Vector2(0, playerRb.velocity.y);
			}
		}
	}

	public void AddWeapon(Weapon weapon){
		canAttack = true;
		weaponEquipped = weapon;
		attack.SetWeapon(weaponEquipped.damage);
	}

	public void Move(float move, bool jump, bool dash)
	{
		if (canMove) {
			if (dash && canDash && !IsFalling)
			{
				//playerRb.AddForce(new Vector2(transform.localScale.x * m_DashForce, 0f));
				StartCoroutine(DashCooldown());
			}
			// If crouching, check to see if the character can stand up
			if (isDashing)
			{
				playerRb.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
			}
			//only control the player if grounded or airControl is turned on
			else if (m_Grounded || m_AirControl)
			{
				if (playerRb.velocity.y < -limitFallSpeed)
					playerRb.velocity = new Vector2(playerRb.velocity.x, -limitFallSpeed);
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, playerRb.velocity.y);
				// And then smoothing it out and applying it to the character
				playerRb.velocity = Vector3.SmoothDamp(playerRb.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight && !IsFalling)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight && !IsFalling)
				{
					// ... flip the player.
					Flip();
				}
			}
			// If the player should jump...
			if (m_Grounded && jump)
			{
				// Add a vertical force to the player.
				playerAnim.SetBool("IsJumping", true);
				playerAnim.SetBool("JumpUp", true);
				m_Grounded = false;
				playerRb.AddForce(new Vector2(0f, m_JumpForce));
				canDoubleJump = true;
			}
			else if (!m_Grounded && jump && canDoubleJump && !IsFalling)
			{
				canDoubleJump = false;
				playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
				playerRb.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
				playerAnim.SetBool("IsDoubleJumping", true);
			}

			else if (m_IsWall && !m_Grounded)
			{
				if (!oldWallSlidding && playerRb.velocity.y < 0 || isDashing)
				{
					IsFalling = true;
					m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
					Flip();
					StartCoroutine(WaitToCheck(0.1f));
					canDoubleJump = true;
					playerAnim.SetBool("IsFalling", true);
				}
				isDashing = false;

				if (IsFalling)
				{
					if (move * transform.localScale.x > 0.1f)
					{
						StartCoroutine(WaitToEndSliding());
					}
					else 
					{
						oldWallSlidding = true;
						playerRb.velocity = new Vector2(-transform.localScale.x * 2, -5);
					}
				}

				if (jump && IsFalling)
				{
					playerAnim.SetBool("IsJumping", true);
					playerAnim.SetBool("JumpUp", true); 
					playerRb.velocity = new Vector2(0f, 0f);
					playerRb.AddForce(new Vector2(transform.localScale.x * m_JumpForce *1.2f, m_JumpForce));
					jumpWallStartX = transform.position.x;
					limitVelOnWallJump = true;
					canDoubleJump = true;
					IsFalling = false;
					playerAnim.SetBool("IsFalling", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canMove = false;
				}
				else if (dash && canDash)
				{
					IsFalling = false;
					playerAnim.SetBool("IsFalling", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canDoubleJump = true;
					StartCoroutine(DashCooldown());
				}
			}
			else if (IsFalling && !m_IsWall && canCheck) 
			{
				IsFalling = false;
				playerAnim.SetBool("IsFalling", false);
				oldWallSlidding = false;
				m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
				canDoubleJump = true;
			}
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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

	IEnumerator WaitToMove(float time)
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator WaitToCheck(float time)
	{
		canCheck = false;
		yield return new WaitForSeconds(time);
		canCheck = true;
	}

	IEnumerator WaitToEndSliding()
	{
		yield return new WaitForSeconds(0.1f);
		canDoubleJump = true;
		IsFalling = false;
		playerAnim.SetBool("IsFalling", false);
		oldWallSlidding = false;
		m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
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
