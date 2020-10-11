using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogIA : MonoBehaviour
{
    private GameController _GameController;
    private Rigidbody2D frogRb;
    private Animator frogAnim;

    public float speed;
    public float timeToWalk;


    // Start is called before the first frame update
    void Start()
    {
        
        _GameController = FindObjectOfType<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
