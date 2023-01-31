using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Sprite[] bar;
    public Image healthBarUI1;
    public Image healthBarUI2;
    public Image healthBarUI3;

    private CharacterController2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find ("Player").GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //Controla primeiro coração
        if(player.life >= 0 && player.life <= 4){
            healthBarUI1.sprite = bar[player.life];
        }
        if(player.life >= 5){
            healthBarUI1.sprite = bar[4];
        }

        // Controla segundo coração
        if(player.life >= 5 && player.life <= 8){
            healthBarUI2.sprite = bar[player.life-4];
        }
        else if(player.life <= 4){
            healthBarUI2.sprite = bar[0];
        }
        if(player.life >= 9){
            healthBarUI2.sprite = bar[4];
        }

        // Controla terceiro coração
        if(player.life >= 9 && player.life <= 12){
            healthBarUI3.sprite = bar[player.life-8];
        }
        else if(player.life <= 8){
            healthBarUI3.sprite = bar[0];
        }
    }
}