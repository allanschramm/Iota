using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public Weapon weapon;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = weapon.image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController2D player = other.GetComponent<PlayerController2D> ();

        if(player !=null){
            player.AddWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
