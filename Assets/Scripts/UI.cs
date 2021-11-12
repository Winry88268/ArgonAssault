using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("UI Arrays")]    
    [Tooltip("Grouping of UI Health Sprites")] [SerializeField] public Image[] health;
    [Tooltip("Grouping of UI Life Sprites")] [SerializeField] public Image[] life;

    // bool isCreated = false;

    // private void Awake()
    // {
    //     if (!isCreated)
    //     {
    //         DontDestroyOnLoad(this.gameObject);
    //         isCreated = true;   
    //     }
    // }

    public void ReduceHealth(int h)
    {
        health[h].GetComponent<Image>().enabled = false;
    }

    public void ReduceLives(int l)
    {
        life[l].GetComponent<Image>().enabled = false;
    }
}
