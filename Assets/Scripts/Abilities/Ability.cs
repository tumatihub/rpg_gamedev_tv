using UnityEngine;
using System.Collections;
using GameDevTV.Inventories;
using System.Collections.Generic;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "RPG/Abilities/Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;

        public override void Use(GameObject user)
        {
            targetingStrategy.StartTargeting(user, (IEnumerable < GameObject > targets) =>
            {
                TargetAquired(user, targets);
            });
        }

        void TargetAquired(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var filterStrategy in filterStrategies)
            {
                targets = filterStrategy.Filter(targets);
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(user, targets, EffectFinished);
            }
        }

        void EffectFinished()
        {

        }
    }
}
