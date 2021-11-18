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
        CharacterController2D player = other.GetComponent<CharacterController2D> ();

        if(player !=null){
            player.AddWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
