using UnityEngine;
using System.Collections;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;
        Mover mover;

        void Awake()
        {
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            if (target != null)
            {
                if (Vector3.Distance(target.position, transform.position) > weaponRange)
                {
                    mover.MoveTo(target.position);
                }
                else
                {
                    mover.Stop();
                    target = null;
                }
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }
    }
}
