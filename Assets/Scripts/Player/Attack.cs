using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float damage;
	public Animator animator;

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
    }

	public void PlayAnimation(AnimationClip clip){ 
		animator.Play(clip.name);
	}

	public void SetWeapon(int damageValue){
		damage = damageValue;
	}

	void OnTriggerEnter2D(Collider2D other) {
		FrogIA enemy = other.GetComponent<FrogIA> ();
		DestructibleObject destrObject = other.GetComponent<DestructibleObject> ();
		
		if(enemy != null){
			enemy.ApplyDamage(damage);
		}

		if(destrObject != null){
			destrObject.ApplyDamage(damage);
		}
	}
}
