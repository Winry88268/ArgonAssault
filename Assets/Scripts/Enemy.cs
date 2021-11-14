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
    [Tooltip("Enemy Hit Points before Destruction")] [SerializeField] float hitPoints = 1f;
    [Tooltip("Point Value for Hitting Enemy")] [SerializeField] float hitValue;
    [Tooltip("Point Value for Killing Enemy")] [SerializeField] int deathValue;

    GameObject vfx;
    GameManager gm;
    Material thisMat;
    Color originalColor;
    
    bool isHit = false, isRunning = false;
    public float power = 1f, totalPoints, pointsGain;

    void Start() 
    {
        gm = FindObjectOfType<GameManager>();
        thisMat = GetComponent<MeshRenderer>().material;
        originalColor = thisMat.color;

        totalPoints = hitPoints * hitValue;
    }

    // If HitFlash is not already running
    // And the target has been Hit
    void Update() 
    {
        if(!isRunning && isHit)
        {
            StartCoroutine("HitFlash");
        }
    }

    // Enemy Collision with Particles
    // I.E. Player Laser Emission
    void OnParticleCollision(GameObject other) 
    {
        Contact();
    }

    // If enemy Trigger-Collides with a GameObject that is not Terrain or another Enemy
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

    // Toggle isRunning
    // Set Enemy Color to Red
    // Wait 0.1 Seconds
    // Set Enemy Color to Original Color
    // Toggle isRunning, Toggle isHit
     public IEnumerator HitFlash()
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

    // Toggle isHit
    // Reduce hitPoints by Laser Power
    // Calculate pointsGain by Laser Power
    // Reduce pointsGain from Enemy totalPoints
    // Increase the Score Board
    // If hitPoints below 0.01f, Disable the Collider and Initiate Destruction of Enemy
    void Contact()
    {
        isHit = true;
        hitPoints -= Mathf.Clamp((power), 0f, 1f);
        pointsGain = Mathf.Clamp((hitValue * power), 0f, totalPoints);
        totalPoints -= pointsGain;
        gm.IncreaseScore(pointsGain);
        if(hitPoints < 0.01f)
        {
            GetComponent<SphereCollider>().enabled = false;
            Destruction();
        }
    }

    // Instantiate an instance of the enemy Explosion FX
    // Assign the FX to the Collector
    // Destroy this Game Object
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