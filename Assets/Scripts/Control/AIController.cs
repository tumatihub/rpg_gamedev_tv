using UnityEngine;
using System.Collections;
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Fighter fighter;
        GameObject player;
        Health health;

        void Awake()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
            }
        }

        bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position) <= chaseDistance;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
