using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : GameBehaviour
{

    public int damage;
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //other.GetComponent<PlayerMovement>().Hit(damage);
            _PLAYER.Hit(damage);
        }
    }
}
