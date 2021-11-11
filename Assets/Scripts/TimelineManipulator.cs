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

    // On ESC press: if(paused) > Unpause, else Pause
    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pd.playableGraph.GetRootPlayable(0).GetSpeed() == 0)
            {
                UnPauseGame();
            }
            else
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

    // Un/Pauses the Timeline
    void PlayAtSpeed(int speed)
    {
        pd.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        pd.Play();
    }
}