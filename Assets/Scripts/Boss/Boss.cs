using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

	public Transform player;

	public bool isAwake = false;
	public float attackDistance;

	public bool isFlipped = false;

	public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

	public float PlayerDistance(){
        return Vector2.Distance(player.position, transform.position);
    }

	private void Update() {
		
		float distance = PlayerDistance();

		isAwake = (distance <= attackDistance);

        if(isAwake){
			transform.GetComponent<Animator>().SetBool("IsAwake", true);
        }
        else{
			transform.GetComponent<Animator>().SetBool("IsAwake", false);
        }
	}

}
