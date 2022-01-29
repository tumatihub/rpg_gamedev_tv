using UnityEngine;
using System.Collections;
using RPG.Attributes;
using System;
using UnityEngine.AI;
using RPG.SceneManagement;
using Cinemachine;

namespace RPG.Control
{
    [RequireComponent(typeof(Health))]
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeTime = .2f;
        [SerializeField] float healthRegenPercentage = 20;
        [SerializeField] float enemyHealthRegenPercentage = 20;
        Health health;

        void Awake()
        {
            health = GetComponent<Health>();
            health.onDie.AddListener(Respawn);
        }

        void Start()
        {
            if (health.IsDead)
            {
                Respawn();
            }
        }

        void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            RespawnPlayer();
            ResetEnemies();
            savingWrapper.Save();
            yield return fader.FadeIn(fadeTime);
        }

        void ResetEnemies()
        {
            foreach (AIController enemyController in FindObjectsOfType<AIController>())
            {
                Health health = enemyController.GetComponent<Health>();
                if (health && !health.IsDead)
                {
                    enemyController.Reset();
                    health.Heal(health.GetMaxHealthPoints() * enemyHealthRegenPercentage / 100);
                }
            }
        }

        void RespawnPlayer()
        {
            Vector3 positionDelta = respawnLocation.position - transform.position;
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }
    }
}