using UnityEngine;
using System.Collections;
using RPG.Movement;
using System;
using RPG.Combat;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;

        Mover mover;
        Fighter fighter;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        void Update()
        {
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

                if (Input.GetMouseButtonDown(0))
                {
                    fighter.Attack(combatTarget);
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
                    mover.MoveTo(hit.point);
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
