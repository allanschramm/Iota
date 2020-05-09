using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D> ().velocity
            = new Vector2 (transform.localRotation.eulerAngles.z == 0 ? 20 : -20, 0);
        Destroy (gameObject, .5f);
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other) {
        if (!other.isTrigger) {
            Destroy (gameObject);
        }       
    }
}
