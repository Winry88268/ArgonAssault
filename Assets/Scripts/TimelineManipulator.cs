using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManipulator : MonoBehaviour
{
    PlayableDirector pd;

    void Awake() 
    {
        pd = GetComponent<PlayableDirector>();
    }

    void Start() 
    {
        PauseGame();
    }

    void Update() 
    {
        if(pd.playableGraph.GetRootPlayable(0).GetSpeed() == 0)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                UnPauseGame();
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        PlayAtSpeed(0);
        Debug.Log("=GAME IS PAUSED. PRESS ESC TO UNPAUSE=");
    }

    void UnPauseGame()
    {
        PlayAtSpeed(1);
        Debug.Log("=GAME IS UNPAUSED. PRESS ESC TO PAUSE=");
    }

    void PlayAtSpeed(int speed)
    {
        pd.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        pd.Play();
    }
}