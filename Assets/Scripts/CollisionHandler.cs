using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("Generic Setup Settings")]
    [Tooltip("Scene reload delay on Player Death")] [SerializeField] float sceneDelay = 2f;
    [Tooltip("Immunity after collision")] [SerializeField] float damageImmunity = 3f;

    // Player Prefab has initial Colliders, which are all Grouped here for ease
    [Header("Collider Array")]
    [Tooltip("Grouping of Player Ship colliders")] [SerializeField] GameObject[] colliders;
    
    [Header("Explosion FX")]
    [Tooltip("Particle FX Systems")] [SerializeField] ParticleSystem playerExplosionFX;
    
    GameManager gm;
    PlayerController pc;
    Rigidbody rb;
    UI canvas; 
    Color originalColor;
    float collisionTime;

    int currentScene, lives, hits;
    // isEnabled tracks if the Player Controls are active
    // isImmune tracks if the Player is currently Immune to Damage
    // isRunning tracks if the immuneFlash coroutine is active
    bool isDead = false, isEnabled = true, isImmune = false, isRunning = false;
    Color immuneColor = Color.white;
    
    
    // Player Controller must be enabled on start, to prevent Laser Particle emission when game is supposed to be paused
    // followed by disabling Player Controller to prevent action when game is supposed to be paused
    // Lastly, checks current lives and modifies UI Images as appropriate
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        canvas = FindObjectOfType<UI>();

        originalColor = GetComponent<Renderer>().material.color;

        lives = gm.curLives;
        hits = gm.maxHits;
        
        TogglePause();

        if(lives<3)
        {
            canvas.ReduceLives(1);
        }

        if(lives<2)
        {
            canvas.ReduceLives(0);
        }
    }

    // if(game is paused) > disable laser particle emitter
    // if ESC is pressed: invert isEnabled > dis/able Player Controller
    void Update() 
    {   
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }

        if(Time.time - collisionTime > damageImmunity)
        {
            isImmune = false;
        }   

        if(isImmune && !isDead)
        {
            if(!isRunning)
            {
                StartCoroutine("immuneFlash");
            }
        }

        if(isDead)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(gameObject);    
                SceneManager.LoadScene(currentScene);
            }
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
        else if(Time.time - collisionTime < damageImmunity)
        {
            return;
        }   

        Hit();
    }

    private void Hit()
    {
        hits--;
        if(hits >= 0)
        {
            canvas.ReduceHealth(hits);
            isImmune = true;
        }
        if(hits == 0)
        {
            Dead();
        }
        collisionTime = Time.time;
    }

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

    void TogglePause()
    {   
        pc.enabled = isEnabled;
        isEnabled = !isEnabled;
        
        if(isEnabled)
        {
            pc.LaserFire(false);
        }

        canvas.pauseToggle();
    }

    // Reduce Player Life count, and Reinitialise Scene based on current Life count
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
            gm.curLives = gm.maxLives;
            gm.score = 0;
            gm.isDead();
            new WaitForSeconds(sceneDelay);
            GetComponent<MeshRenderer>().enabled = false;
            return;
        }

        Destroy(gameObject);    
        SceneManager.LoadScene(currentScene);
    }
}