﻿using UnityEngine;
using System.Collections;
using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(ActionScheduler))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWaypointIndex = 0;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;
        ActionScheduler actionScheduler;

        void Awake()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = transform.position;
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            if (health.IsDead) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMovingAction(nextPosition);
            }
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex);
        }

        bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position) <= chaseDistance;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}