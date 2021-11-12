using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool isCreated = false;

    [Header("Persistent Settings")]
    [Tooltip("Player Hits before Destruction")] [SerializeField] public int maxHits = 3;
    [Tooltip("Maximum Number of Deaths before Game reset")] [SerializeField] public int maxLives = 3;
    [Tooltip("The UI element")] [SerializeField] public UI canvas;    

    [SerializeField] public int curLives, curHits;
    [SerializeField] public int score;

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

    public void IncreaseScore(int killValue) 
    {
        score += killValue;
    }

    public void ReduceHealth(int h)
    {
        canvas.ReduceHealth(h);
    }

    public void ReduceLives(int l)
    {
        canvas.ReduceLives(l);
    }

    public void GetCanvas()
    {
        canvas = FindObjectOfType<UI>();
        if(curLives<3)
        {
            ReduceLives(1);
        }

        if(curLives<2)
        {
            ReduceLives(0);
        }
    }
}