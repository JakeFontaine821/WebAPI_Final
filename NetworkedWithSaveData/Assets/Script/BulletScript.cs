using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickStart;

public class BulletScript : MonoBehaviour
{
    public GameObject shotby;
    float lifespan = 5f;

    public void Update()
    {
        lifespan -= Time.deltaTime;
        if(lifespan <= 0)
        {
            DestroyImmediate(gameObject, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != shotby)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerScript>().AddImpact(gameObject.GetComponent<Rigidbody>().velocity, 5f);
                other.GetComponent<PlayerScript>().AddToScore(-1);
            }
            else if (other.CompareTag("Target"))
            {
                shotby.GetComponent<PlayerScript>().AddToScore(2);
                other.GetComponent<TargetScript>().NewPostion();
            }
        }
    }
}
