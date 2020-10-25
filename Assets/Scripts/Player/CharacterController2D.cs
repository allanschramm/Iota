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
	public GameObject throwableObject;
	public bool canAttack = true; // Controla se o jogador pode atacar

	// Movimentação
	private bool canMove = true; //If player can move
	private float h;

	// Dash control
	[SerializeField] private float m_DashForce = 25f;
	private bool canDash = true; // Controla de o jogador pode usar o Dash
	private bool isDashing = false; //If player is dashing
	public Transform attackCheck; // Verifica a colisdão do dash na frente do jogador

	public bool invincible = false; //If player can die

	// Camera
	public GameObject cam;

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



	private void Update(){

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
		
		if(canMove){
			h = Input.GetAxisRaw("Horizontal") * speed;
		}

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

        if(Input.GetButtonDown("Fire1") && isAttacking == false && canAttack){
            playerAnim.SetTrigger("Attack");
            _GameController.PlaySFX(_GameController.sfxAttack, 1f);
            isAttacking = true;
            attack.PlayAnimation(weaponEquipped.animation);
			StartCoroutine(AttackCooldown());
        }

		if (Input.GetButton("Fire2"))
		{
			dash = true;
			if (dash && canDash){
				StartCoroutine(DashCooldown());
				playerRb.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
			}
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

	void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Collectable"){
            _GameController.PlaySFX(_GameController.sfxWeapons, 1f);
        }
	}

	public void AddWeapon(Weapon weapon){
		canAttack = true;
		weaponEquipped = weapon;
		attack.SetWeapon(weaponEquipped.damage);
	}

	public void AddKey(){
		keys++;
	}

	void Dash(){
		
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
			// Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f ;
			// playerRb.velocity = Vector2.zero;
			// playerRb.AddForce(damageDir * 50);
			if (life <= 0)
			{
				StartCoroutine(WaitToDead());
			}
			else
			{
				StartCoroutine(Stun(0.1f));
				StartCoroutine(MakeInvincible(2f));
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
		playerAnim.SetTrigger("IsDashing");
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
