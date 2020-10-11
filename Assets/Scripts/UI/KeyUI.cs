using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Sprite[] bar;
    public Image Key;

    private CharacterController2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find ("Player").GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Key.sprite = bar[player.keys];
    }
}
