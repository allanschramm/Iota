using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogIA : MonoBehaviour
{
    private GameController _GameController;
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
        
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;

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

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "HitBox"){
            h = 0;
            StopCoroutine("frogWalk");
            Destroy(HitBox);
            // _GameController.PlaySFX(_GameController.sfxEnemyDead, 0.2f);
            frogAnim.SetTrigger("IsDead");
            Debug.Log("Sapo tomou dano!");
        }
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

    void OnDead(){
        Destroy(this.gameObject);
    }

    void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

    }
}
