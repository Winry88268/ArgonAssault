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
    [SerializeField] Slider slider;
    [SerializeField] Image sliderBar;
    Gradient gradient;
    GradientColorKey[] colorKeys;
    GradientAlphaKey[] alphaKeys;
    RectTransform rt;
    GameManager gm;

    // is PAUSE currently active (Inverts on Start)
    // is the Player Dead (Inverts on Start)
    bool isActive = true, isDead = true;
    float sliderMax;

    void Start() 
    {
        generateGradient();

        GameOverToggle();
        pauseToggle();

        rt = slider.GetComponent<RectTransform>();
        gm = FindObjectOfType<GameManager>();

        gm.getHandles();
        setScore(gm.score);
        slider.value = 1f;  
    }

    void Update() 
    {
        sliderBar.color = gradient.Evaluate(slider.value);
        gm.laserColor(sliderBar.color);
    }

    void generateGradient()
    {
        gradient = new Gradient();

        colorKeys = new GradientColorKey[3];
        colorKeys[0].color = Color.red;
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = Color.yellow;
        colorKeys[1].time = 0.5f;
        colorKeys[2].color = Color.green;
        colorKeys[2].time = 1.0f;

        alphaKeys = new GradientAlphaKey[3];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 0.5f;
        alphaKeys[2].alpha = 1.0f;
        alphaKeys[2].time = 1.0f;

        gradient.SetKeys(colorKeys, alphaKeys);
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
                    i.enabled = false;
                }
                else
                {
                    i.enabled = true;
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

    public void LaserPowerUpdate(float powerValue)
    {
        slider.value = powerValue;
    }
}
