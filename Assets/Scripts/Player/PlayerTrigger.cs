using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{

    private CharacterController2D player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find ("Player").GetComponent<CharacterController2D>();
    }

    void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Enemy")){
			EnemyController enemy = other.GetComponent<EnemyController> ();

            if(!player.invincible){
                player.ApplyDamage(enemy.damage);
            }
        }

        if (other.CompareTag ("Portal") && player.GetKey() >= 1){
            Invoke ("NextLevel", 1f);
        }
	}

    void NextLevel(){
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
