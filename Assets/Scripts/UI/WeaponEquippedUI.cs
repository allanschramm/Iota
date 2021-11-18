using UnityEngine;

public class WeaponEquippedUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] bar;
    public Image WeaponUI;

    private CharacterController2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find ("Player").GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponUI.sprite = bar[player.weaponEquipped.itemID];
    }
}
