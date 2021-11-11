using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("System Assignments")]
    [Tooltip("Enemy FX upon taking Damage")] [SerializeField] GameObject enemyExplosionFX; 
    [Tooltip("Collector for short lifespan Instantiated Game Objects")] [SerializeField] Transform parent;

    GameObject vfx;

    void OnParticleCollision(GameObject other) 
    {
        Contact();
    }

    // If enemy collides with a GameObject that is not Terrain or another Enemy
    // I.E. The Player
    // Behave as if the enemy was hit by a Laser Particle
    // If the collision was with Terrain or another Enemy
    // Do nothing
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name !="Terrain" || other.gameObject.name !="Enemy")
        {
            Contact();
        }
        else return;
    }

    // Instantiate an instance of the enemy Explosion FX
    // Assign the FX to the Collector
    // Destroy this Game Object
    void Contact()
    {
        vfx = Instantiate(enemyExplosionFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        Destroy(gameObject);
    }    
}