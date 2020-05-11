using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifesCount : MonoBehaviour
{
    public Sprite[] life;
    public Image lifesCountUI;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find ("Alien").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        lifesCountUI.sprite = life[player.lifes];
    }
}
