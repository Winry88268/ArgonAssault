using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("Generic Setup Settings")]
    [Tooltip("Scene reload delay on Player Death")] [SerializeField] float sceneDelay = 2f;
    [Tooltip("Immunity after collision")] [SerializeField] float damageImmunity = 2.5f;

    // Player Prefab has initial Colliders, which are all Grouped here for ease
    [Header("Collider Array")]
    [Tooltip("Grouping of Player Ship colliders")] [SerializeField] GameObject[] colliders;
    
    [Header("Explosion FX")]
    [Tooltip("Particle FX Systems")] [SerializeField] ParticleSystem playerExplosionFX;
    
    GameManager gm;
    PlayerController pc;
    Rigidbody rb;
    UI canvas; 
    Color originalColor, immuneColor = Color.white;
    float collisionTime;

    // isImmune tracks if the Player is currently Immune to Damage
    // isRunning tracks if the immuneFlash coroutine is active    
    bool isDead = false, isImmune = false, isRunning = false, gameOver = false;
    int currentScene, lives, hits;  

    // Set Lives and HitPoints from GameManager Persistent Settings
    // Modifies UI Images as appropriate
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        canvas = FindObjectOfType<UI>();

        originalColor = GetComponent<Renderer>().material.color;

        lives = gm.curLives;
        hits = gm.maxHits;
        
        if(lives<3)
        {
            canvas.ReduceLives(1);
        }

        if(lives<2)
        {
            canvas.ReduceLives(0);
        }
    }

    // If Player Immune, and damageImmunity amount of time has passed, remove Player Immunity
    // If Player is Immune and is Not Dead: If immuneFLash is not already running, start immuneFlash
    // If Player gameOver, Restart game on ESC press
    void Update() 
    {   
        if(isImmune)
        {
            if(Time.time - collisionTime > damageImmunity)
            {
                isImmune = false;
            }   
        }
        
        if(isImmune && !isDead)
        {
            if(!isRunning)
            {
                StartCoroutine("immuneFlash");
            }
        }

        if(gameOver)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {

                gm.curLives = gm.maxLives;
                gm.score = 0;
                Destroy(gameObject);    
                SceneManager.LoadScene(currentScene);
                Time.timeScale = 1;
            }
        }
    }

    // If Player is Dead or Immune, ignore Trigger events
    void OnTriggerEnter(Collider other)
    {
        if(isDead)
        {
            return;
        }
        else if(isImmune)
        {
            return;
        }   

        Hit();
    }

    // Reduce Hit Counter and Set Player to Immune
    // If Player is still Alive: modify UI, If Player is Dead: begin Destruction
    private void Hit()
    {
        hits--;
        isImmune = true;
        
        if(hits >= 0)
        {
            canvas.ReduceHealth(hits);
        }
        if(hits < 1)
        {
            Dead();
        }
        collisionTime = Time.time;
    }

    // Toggle Player Dead
    // Disable Player Controls and Lasers
    // Enable RigidBody Gravity to Allow Ship to Crash
    // Toggle all Player Colliders to Enable Ship to Rebound on Objects
    // Start Death Particle Emissions
    // Begin Reload after sceneDelay
    void Dead()
    {
        isDead = true;

        pc.LaserFire(false);
        pc.enabled = false;
        rb.useGravity = true;

        DeathRagdoll();
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

    // Toggle isRunning to prevent multiple instances of immuneFlash at the same time.
    // Disable, wait, Enable: Causes the Player to Flash
    // Wait, isRunning Toggle, StopCorputine: Stops the current instance and allows a new instance to start
    public IEnumerator immuneFlash()
    {
        isRunning = true;
        GetComponent<MeshRenderer>().enabled = false;     
        yield return new WaitForSeconds(0.1f);
        GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        isRunning = false;
        StopCoroutine("immuneFlash");
    }

    // Reduce Player Life count
    // If Player still has Lives: Modify the UI and Restart Current Scene
    // If Player has no Lives, Toggle gameOver
    void Reload()
    {
        lives--;
        gm.curLives = lives;

        if(lives>0)
        {
            canvas.ReduceLives(lives-1);
            currentScene = SceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            currentScene = 0;
            gm.isDead();
            new WaitForSeconds(sceneDelay);
            GetComponent<MeshRenderer>().enabled = false;
            gameOver = true;
            return;
        }

        Destroy(gameObject);    
        SceneManager.LoadScene(currentScene);
    }
}