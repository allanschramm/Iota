﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeGrab : MonoBehaviour
{
    public int GunNumber;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag ("Player")) {
            other.GetComponent<PlayerController> ().EnableGun (GunNumber);
            Destroy (gameObject);
        }
    }
}
