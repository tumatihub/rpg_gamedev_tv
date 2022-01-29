using UnityEngine;
using System.Collections;
using RPG.Attributes;
using System;
using UnityEngine.AI;
using RPG.SceneManagement;

namespace RPG.Control
{
    [RequireComponent(typeof(Health))]
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeTime = .2f;
        [SerializeField] float healthRegenPercentage = 20;
        Health health;

        void Awake()
        {
            health = GetComponent<Health>();
            health.onDie.AddListener(Respawn);
        }

        void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            yield return fader.FadeIn(fadeTime);

        }
    }
}