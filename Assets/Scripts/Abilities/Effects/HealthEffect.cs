using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RPG.Attributes;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "RPG/Abilities/Effects/Health")]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health != null)
                {
                    if (healthChange < 0)
                    {
                        health.TakeDamage(data.GetUser(), -healthChange);
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }
                }
            }
            finished();
        }
    }
}
