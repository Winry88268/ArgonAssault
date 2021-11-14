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
    [SerializeField] public int score = 0;

    UI canvas;

    public bool isPaused = false;

    // Sets this script as Persistent on Scene Load
    private void Awake()
    {
        if (!isCreated)
        {
            DontDestroyOnLoad(this.gameObject);
            isCreated = true;   
        }
    }

    void Start() 
    {
        curLives = maxLives;
        curHits = maxHits;
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
    }

    public void getHandles()
    {
        canvas = FindObjectOfType<UI>();
    }

    public void IncreaseScore(int killValue) 
    {
        score += killValue;
        canvas.setScore(score);
    }

    public void isDead()
    {
        canvas.GameOverToggle();
    }

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

    public void powerUpdate(float power)
    {
        canvas.LaserPowerUpdate(power);
    }
}