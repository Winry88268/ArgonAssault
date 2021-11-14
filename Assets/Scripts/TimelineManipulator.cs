// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Playables;

// public class TimelineManipulator : MonoBehaviour
// {
//     PlayableDirector pd;

//     public bool isDead = false;

//     void Awake() 
//     {
//         pd = GetComponent<PlayableDirector>();
//     }

//     // On ESC press: if(paused) > Unpause, else Pause
//     void Update() 
//     {
//         if(Input.GetKeyDown(KeyCode.Space))
//         {
//             if (pd.playableGraph.GetRootPlayable(0).GetSpeed() == 0 && !isDead)
//             {
//                 UnPauseGame();
//             }
//             else
//             {
//                 PauseGame();
//             }              
//         }
//     }

//     public void PauseGame()
//     {
//         PlayAtSpeed(0);
//     }

//     void UnPauseGame()
//     {
//         PlayAtSpeed(1);
//     }

//     // Un/Pauses the Timeline
//     void PlayAtSpeed(int speed)
//     {
//         pd.playableGraph.GetRootPlayable(0).SetSpeed(speed);
//         pd.Play();
//     }
// }