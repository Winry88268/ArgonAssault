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

    public void getCanvas()
    {
        canvas = FindObjectOfType<UI>();
    }

    public void IncreaseScore(int killValue) 
    {
        score += killValue;
        canvas.setScore(score);
    }
}