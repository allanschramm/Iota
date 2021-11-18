using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    public GameObject semChave;
    // Start is called before the first frame update
    void Start()
    {
        semChave.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        CharacterController2D player = other.GetComponent<CharacterController2D> ();

        if(player !=null && player.GetKey() <= 0){
            semChave.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        semChave.SetActive(false);
    }
}
