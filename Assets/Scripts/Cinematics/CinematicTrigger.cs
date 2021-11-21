using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        PlayableDirector playableDirector;
        bool wasPlayed = false;

        void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!wasPlayed && other.gameObject.CompareTag("Player"))
            {
                playableDirector.Play();
                wasPlayed = true;
            }
        }
    }
}
