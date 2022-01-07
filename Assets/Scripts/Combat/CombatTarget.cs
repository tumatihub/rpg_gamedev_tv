﻿using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled) return false;

            Fighter fighter = callingController.GetComponent<Fighter>();
            if (!fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            {
                fighter.Attack(gameObject);
            }
            return true;
        }
    }
}
