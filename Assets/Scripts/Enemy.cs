using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnParticleCollision(GameObject other) 
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name !="Terrain" || other.gameObject.name !="Enemy")
        {
            Destroy(gameObject);
        }
        else return;
    }    
}