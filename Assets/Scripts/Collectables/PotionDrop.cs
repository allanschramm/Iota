using UnityEngine;

public class PotionDrop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        CharacterController2D player = other.GetComponent<CharacterController2D> ();

        if(player !=null){
            player.AddPotion();
            Destroy(gameObject);
        }
    }
}
