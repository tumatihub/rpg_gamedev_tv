using UnityEngine;
using System.Collections;

namespace RPG.Abilities
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract void StartTargeting();
    }
}
