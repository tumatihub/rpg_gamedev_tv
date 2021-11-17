using UnityEngine;
using System.Collections;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {

        public void Attack(CombatTarget combatTarget)
        {
            print($"take that! {combatTarget.name}");
        }
    }
}
