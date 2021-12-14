using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterEffect : MonoBehaviour
    {
        ParticleSystem particleSystem;

        void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (!particleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
