using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FiringPoint : MonoBehaviour
{
    [Header("Rigid Body Projectiles")]
    public GameObject projectileGreenOrb;
    public float projectileSpeed = 1000f;
    [Header("Raycast Projectiles")]
    public GameObject hitSparkle;
    public LineRenderer laser;


    // Start is called before the first frame update
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            FireRigidBody();
        }

        if(Input.GetButtonDown("Fire2"))
            FireRaycast(); 
    }

    // Update is called once per frame
    void FireRigidBody()
    {
        //Create a reference to hold our instantiated object
        GameObject projectileInstance;
        //Instantiate our projectile at this object position and rotation
        projectileInstance = Instantiate(projectileGreenOrb, transform.position, transform.rotation);
            //Add force to the projectile
            projectileInstance.GetComponent<Rigidbody>().AddForce
            (transform.forward * projectileSpeed);
    }

    void FireRaycast()
    {
        //Create the ray
        Ray ray = new Ray(transform.position, transform.forward);
        //Create a reference to hold the info on what we hit
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log(" He hit " + hit.collider.name + " at point " + hit.point + " which was " + hit.distance + " units away");

            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, hit.point);
            StopAllCoroutines();
            StartCoroutine(StopLaser());

            GameObject particles = Instantiate(hitSparkle, hit.point, hit.transform.rotation);
            Destroy(particles, 1);

            if(hit.collider.CompareTag("Target"))
            {
                Destroy(hit.collider.gameObject);
            }

        }
    }

    IEnumerator StopLaser()
    {
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        laser.gameObject.SetActive(false);
    }

}
