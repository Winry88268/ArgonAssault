using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("Generic Setup Settings")]
    [Tooltip("Scene reload delay on Player Death")] [SerializeField] float sceneDelay = 2f;
    [Tooltip("Player Hits before Destruction")] [SerializeField] int hitPoints = 3;

    // Player Prefab has initial Colliders, which are all Grouped here for ease
    [Header("Collider Array")]
    [Tooltip("Grouping of Player Ship colliders")] [SerializeField] GameObject[] colliders;
    
    [Header("Explosion FX")]
    [Tooltip("Particle FX Systems")] [SerializeField] ParticleSystem playerExplosionFX;
    
    GameManager gm;
    PlayerController pc;
    Rigidbody rb;
    
    int currentScene, lives;
    bool isDead = false;
    bool isEnabled = false;
    
    // Player Controller must be enabled on start, to prevent Laser Particle emission when game is supposed to be paused
    // followed by disabling Player Controller to prevent action when game is supposed to be paused
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        lives = gm.curLives;

        pc.enabled = isEnabled;
    }

    // if(game is paused) > disable laser particle emitter
    // if ESC is pressed: invert isEnabled > dis/able Player Controller
    void Update() 
    {   
        if(!isEnabled)
        {
            pc.LaserFire(false);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isEnabled = !isEnabled;
            pc.enabled = isEnabled;
        }
    }

    // If Player hits Enemy or Terrain, reduce Player HitPoints
    // If Player HitPoints reaches 0: Prevent Player Movement, 
    //                                Enable Player Gravity, 
    //                                Enable Player Collisions 
    void OnTriggerEnter(Collider other) 
    {        
        if(isDead)
        { 
            return;
        }
        
        hitPoints--;

        if(hitPoints == 0)
        {
            pc.enabled = false;
            rb.useGravity = true; 

            DeathRagdoll();
        }
    }

    // On Player Death, play Explosion upon further Collisions
    void OnCollisionEnter(Collision other) 
    {
        if(isDead)
        { 
            return;
        }

        isDead = true;

        DeathExplosion();
        Invoke("Reload", sceneDelay);        
    }

    // Switches collision event types on all Player Colliders
    void DeathRagdoll()
    {
        foreach(GameObject i in colliders)
        {
            i.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    // Play Explosion effect
    void DeathExplosion()
    {
        playerExplosionFX.Play();
    }

    // Reduce Player Life count, and Reinitialise Scene based on current Life count
    void Reload()
    {
        lives--;
        gm.curLives = lives;
        if(lives>0)
        {
            currentScene = SceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            currentScene = 0;
            Debug.Log("--GAME OVER--");
            gm.curLives = lives;
        }
        Destroy(gameObject);    
        SceneManager.LoadScene(currentScene);
    }
}