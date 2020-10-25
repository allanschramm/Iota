using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public int damage;
	public Animator anim;


	// Start is called before the first frame update
	void Start()
    {
        anim = GetComponent<Animator>();
    }

	public void PlayAnimation(AnimationClip clip){ 
		anim.Play(clip.name);
	}

	public void SetWeapon(int damageValue){
		damage = damageValue;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Enemy") || other.CompareTag("Boss")){

			// Aplica dano aos inimigos comuns
			EnemyController enemy = other.GetComponent<EnemyController> ();
		
			if(enemy != null){
				enemy.DamageEnemy(damage);
			}

			// Aplica dano ao Boss
			BossHealth bossHealth = other.GetComponent<BossHealth>();

			if(bossHealth != null){
				bossHealth.TakeDamage(damage);
			}

			// Aplica dano aos objetos destrutiveis
			DestructibleObject destrObject = other.GetComponent<DestructibleObject> ();

			if(destrObject != null){
				destrObject.ApplyDamage(damage);
			}
		}


	}
}
