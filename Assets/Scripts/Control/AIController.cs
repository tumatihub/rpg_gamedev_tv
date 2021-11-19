using UnityEngine;
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

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

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
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        void GuardBehaviour()
        {
            mover.StartMovingAction(guardPosition);
        }

        void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        void AttackBehaviour()
        {
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
