using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform AttackRange;
    public GameObject SlashPrefab;
    public float AttackInterval = 0.25f;

    private bool doAttack = false;
    private float nextAttack = 0;

    // Update is called once per frame
    void Update()
    {
        if (doAttack && Time.time >= nextAttack){
            // programar função do attack aqui
            Instantiate (SlashPrefab, AttackRange.position,
                Quaternion.Euler(0, 0, transform.lossyScale.x < 0 ? -180 :0));
            nextAttack = Time.time + AttackInterval;
        }
    }

    public void Attack(){
        doAttack = true;
    }
}
