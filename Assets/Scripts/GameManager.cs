using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool isCreated = false;

    [Header("Persistent Settings")]
    [Tooltip("Player Hits before Destruction")] [SerializeField] public int maxHits = 3;
    [Tooltip("Maximum Number of Deaths before Game reset")] [SerializeField] public int maxLives = 3;   

    [SerializeField] public int curLives, curHits;
    [SerializeField] public float score = 0;

    UI canvas;
    PlayerController pc;
    public List<GameObject> nmy;

    public bool isPaused = false;

    // Sets this script as Persistent on Scene Load
    private void Awake()
    {
        if (!isCreated)
        {
            DontDestroyOnLoad(this.gameObject);
            isCreated = true;  

            curLives = maxLives;
            curHits = maxHits;
        }
    }

    void Update() 
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
    }

    // Collect necessary Handles on other Game Scripts After Scene Load
    public void getHandles()
    {
        canvas = FindObjectOfType<UI>();
        pc = FindObjectOfType<PlayerController>();
    }

    // Increase persistent Score and call UI Update
    public void IncreaseScore(float Value) 
    {
        score += Value;
        canvas.setScore(score);
    }

    // Call UI Game Over display and stop all game elements
    public void isDead()
    {
        canvas.GameOverToggle();
        Time.timeScale = 0;
    }

    // Call Toggle UI Pause display, and Toggle game Un/Pause
    public void TogglePause()
    {
        canvas.pauseToggle();
        if(isPaused)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        isPaused = !isPaused;
    }

    // Call UI Laser Power Update
    // Update Enemy with current Laser Modifier
    public void powerUpdate(float power)
    {
        canvas.LaserPowerUpdate(power);
        if(nmy != null)
        {
            foreach(GameObject i in nmy)
            {   
                i.GetComponent<Enemy>().power = power; 
            }
        }   
    }

    // Call PlayerController for Laser Color Update
    public void laserColor(Color color)
    {
        foreach(GameObject i in pc.lasers)
        {
            ParticleSystem.MainModule j = i.GetComponent<ParticleSystem>().main;
            j.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }
}