using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

	public int health = 80;

	// public GameObject deathEffect;

	public bool isInvulnerable = false;

	public void TakeDamage(int damage)
	{

		if (isInvulnerable)
			return;

		health -= damage;
		Debug.Log(health);
		StartCoroutine(HitTime());

		if (health <= 30)
		{
			GetComponent<Animator>().SetBool("IsEnraged", true);
		}

		if (health <= 0)
		{
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}
		else{
		}
	}

	// void Die()
	// {
	// 	Instantiate(deathEffect, transform.position, Quaternion.identity);
	// 	Destroy(gameObject);
	// }

	
	IEnumerator HitTime()
	{
		Debug.Log("Boss tomou hit");
		isInvulnerable = true;
        transform.GetComponent<Animator>().SetTrigger("Hit");
		yield return new WaitForSeconds(0.3f);
		isInvulnerable = false;
		Debug.Log(isInvulnerable);
	}
    IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		// Boss_Run bossRun = GetComponent<Boss_Run>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		// bossRun.rb.velocity = new Vector2(0, bossRun.rb.velocity.y);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
