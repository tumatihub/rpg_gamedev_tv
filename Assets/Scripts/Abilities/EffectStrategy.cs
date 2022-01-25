using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Abilities
{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void StartEffect(AbilityData data, Action finished);
    }
}
