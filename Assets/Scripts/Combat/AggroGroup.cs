using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activateOnStart = false;

        void Start()
        {
            Activate(activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach(Fighter fighter in fighters)
            {
                fighter.enabled = shouldActivate;
                CombatTarget target = fighter.GetComponent<CombatTarget>();
                if (target != null) target.enabled = shouldActivate;
            }
        }
    }
}
