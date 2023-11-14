using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
  
{   //Week 5
    public int damage = 20;
    
    void Start()
    {
        Destroy(this.gameObject,3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check If we hit the tagged Target

        if(collision.gameObject.CompareTag("Target"))
        {

            if(collision.gameObject.GetComponent<Target>() != null)
            {
                collision.gameObject.GetComponent<Target>().Hit();
            }//Week 7

            //Chancge the colour of the target
            //collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
            //Destroy the target after 1 second
            //Destroy(collision.gameObject, 1f);
            //Destroy this object
            //Destroy(this.gameObject);
        }

        
    }
}
