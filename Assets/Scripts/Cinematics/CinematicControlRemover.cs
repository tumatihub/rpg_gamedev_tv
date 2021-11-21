using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicControlRemover : MonoBehaviour
    {
        PlayableDirector playableDirector;

        void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        void Start()
        {
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            print("Disable Control");
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            print("Enable Control");
        }

        void OnDestroy()
        {
            playableDirector.played -= DisableControl;
            playableDirector.stopped -= EnableControl;
        }
    }
}
