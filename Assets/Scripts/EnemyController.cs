using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public WeaponController EnemyWeapon;
    public float WalkDistance;
    private float minX;
    private float maxX;

    private float destinationX;
    private Rigidbody2D rb2d;

    private int direction = -1;
    public int speed;

    private bool attacking = false;
    private GameObject player;

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

            if (EnemyWeapon != null){
                EnemyWeapon.Attack();
            }

            attacking = Mathf.Abs (player.transform.position.x - transform.position.x) < 10;

        } else {
            Move();
        }
    }

    void Move(){
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

    void Flip(){
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void AttackPlayer(){
        attacking = true;
    }
}
