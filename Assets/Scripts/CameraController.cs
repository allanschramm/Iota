using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag ("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null){
            Vector3 newPosition = transform.position;
            newPosition.x = player.transform.position.x;

            transform.position = newPosition;
        }
    }
}
