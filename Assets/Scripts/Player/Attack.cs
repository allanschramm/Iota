using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public int damage;
	public Animator attackAnim;

	// Start is called before the first frame update
	void Start()
    {
        attackAnim = GetComponent<Animator>();
    }

	public void PlayAnimation(AnimationClip clip){ 
		attackAnim.Play(clip.name);
	}

	public void SetWeapon(int damageValue){
		damage = damageValue;
	}

	void OnTriggerEnter2D(Collider2D other) {
		EnemyController enemy = other.GetComponent<EnemyController> ();
		DestructibleObject destrObject = other.GetComponent<DestructibleObject> ();
		
		if(enemy != null){
			enemy.ApplyDamage(damage);
			Debug.Log("Aplicou dano no inimigo");
		}

		if(destrObject != null){
			destrObject.ApplyDamage(damage);
		}
	}
}
