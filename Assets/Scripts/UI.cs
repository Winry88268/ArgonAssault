using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [Header("UI Arrays")]    
    [Tooltip("Grouping of UI Health Sprites")] [SerializeField] public Image[] health;
    [Tooltip("Grouping of UI Life Sprites")] [SerializeField] public Image[] life;
    [Tooltip("Grouping of Pause texts")] [SerializeField] public TextMeshProUGUI[] pause;
    [Tooltip("Grouping of Game Over texts")] [SerializeField] public TextMeshProUGUI[] over;

    [Tooltip("Score Text")] [SerializeField] TextMeshProUGUI scoreCounter;
    
    // is PAUSE currently active
    bool isActive = true, isDead = true;

    GameManager gm;

    void Start() 
    {
        pauseToggle();
        gm = FindObjectOfType<GameManager>();

        gm.getHandles();
        setScore(gm.score);

        GameOverToggle();
    }

    public void ReduceHealth(int h)
    {
        health[h].GetComponent<Image>().enabled = false;
    }

    public void ReduceLives(int l)
    {
        life[l].GetComponent<Image>().enabled = false;
    }

    public void pauseToggle()
    {   
        if(!isDead)
        {
            isActive = !isActive;
            foreach(TextMeshProUGUI i in pause)
            {
                if(!isActive)
                {
                    i.enabled = true;
                }
                else
                {
                    i.enabled = false;
                }    
            }
        }  
    }

    public void setScore(int i)
    {
        scoreCounter.text = i.ToString();
    }

    public void GameOverToggle()
    {   
        isDead = !isDead;
        foreach(TextMeshProUGUI i in over)
        {
            if(isDead)
            {
                i.enabled = true;
            }
            else
            {
                i.enabled = false;
            }
        }  
    }
}
