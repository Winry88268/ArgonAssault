using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool isCreated = false;

    [SerializeField] private int maxLives = 3;

    public int curLives;

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
    }

    public int getLives()
    {
        return curLives;
    }

    public void setLives(int i)
    {
        curLives = i;
    }
}