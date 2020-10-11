using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquippedUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Weapon weaponEquipped;
    private SpriteRenderer sprite;

    public Sprite bar;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = weaponEquipped.image;
    }

    // Update is called once per frame
    void Update()
    {
        bar = sprite.sprite;
    }
    public void AddWeapon(Weapon weapon){
		weaponEquipped = weapon;
	}
}
