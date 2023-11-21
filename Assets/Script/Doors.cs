using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other. CompareTag ("Enemy"))
        {
          door1.SetActive(false);
          door2.SetActive(false);
        }
       
    }

    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            door1.SetActive(true);
            door2.SetActive(true);
        }
    }
}
