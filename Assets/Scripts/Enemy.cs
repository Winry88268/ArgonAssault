using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("System Assignments")]
    [Tooltip("Enemy FX upon taking Damage")] [SerializeField] GameObject enemyExplosionFX; 
    [Tooltip("Collector for short lifespan Instantiated Game Objects")] [SerializeField] Transform parent;
    [SerializeField] float emissionMultiply = 1f;

    [Header("Enemy Variable Settings")]
    [Tooltip("Enemy Hit Points before Destruction")] [SerializeField] int hitPoints = 1;
    [Tooltip("Point Value for Hitting Enemy")] [SerializeField] int hitValue;
    [Tooltip("Point Value for Killing Enemy")] [SerializeField] int deathValue;

    GameObject vfx;
    //GameObject[] Player;
    GameManager gm;
    Material thisMat;
    Color originalColor;
    
    bool isHit = false, isRunning = false;

    void Start() 
    {
        gm = FindObjectOfType<GameManager>();
        thisMat = GetComponent<MeshRenderer>().material;
        originalColor = thisMat.color;

        //Player = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update() 
    {
        if(!isRunning && isHit)
            {
                StartCoroutine("immuneFlash");
            }
    }

    void OnParticleCollision(GameObject other) 
    {
        if(gameObject.tag != "Projectile")
        {
            Contact();
        }    
    }

    // If enemy collides with a GameObject that is not Terrain or another Enemy
    // I.E. The Player
    // Behave as if the enemy was hit by a Laser Particle
    // If the collision was with Terrain or another Enemy
    // Do nothing
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag !="Terrain" || other.gameObject.tag !="Enemy")
        {
            Contact();
        }
        else return;
    }

     public IEnumerator immuneFlash()
    {
        isRunning = true;
        thisMat.color = Color.red;
        thisMat.SetColor("_EmissionColor", new Vector4(1,0,0,1) * emissionMultiply);     
        yield return new WaitForSeconds(.1f);
        GetComponent<MeshRenderer>().material.color = originalColor;
        isRunning = false;
        isHit = false;
        StopCoroutine("immuneFlash");
    }

    // Instantiate an instance of the enemy Explosion FX
    // Assign the FX to the Collector
    // Destroy this Game Object
    void Contact()
    {
        isHit = true;
        hitPoints--;
        gm.IncreaseScore(hitValue);
        if(hitPoints < 1)
        {
            GetComponent<SphereCollider>().enabled = false;
            Destruction();
        }
        
    }

    void Destruction()
    {
        gm.IncreaseScore(deathValue);
        vfx = Instantiate(enemyExplosionFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        Invoke("Delete", 0.1f);
    }

    void Delete()
    {
        Destroy(gameObject);
    }    
}