using UnityEngine;
using System.Collections;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;

        Mover mover;
        Fighter fighter;
        Health health;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead) return;

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget combatTarget = hit.collider.GetComponent<CombatTarget>();
                if (combatTarget == null) continue;

                if (!fighter.CanAttack(combatTarget.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(combatTarget.gameObject);
                }
                return true;
            }
            return false;
        }

        bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMovingAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        Ray GetMouseRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
