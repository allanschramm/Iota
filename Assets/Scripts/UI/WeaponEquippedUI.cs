using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquippedUI : MonoBehaviour
{
    // Start is called before the first frame update
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
}
