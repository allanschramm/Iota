using UnityEngine;

public class SickleGrab : MonoBehaviour
{
    public int WeaponNumber;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag ("Player")) {
            // other.GetComponent<PlayerMovement> ().EnableGun (WeaponNumber);
            Destroy (gameObject);
        }
    }
}