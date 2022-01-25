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
        [SerializeField] float cooldownTime = 0f;

        public override void Use(GameObject user)
        {
            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0)
            {
                return;
            }

            AbilityData data = new AbilityData(user);
            targetingStrategy.StartTargeting(data, () =>
            {
                TargetAquired(data);
            });
        }

        void TargetAquired(AbilityData data)
        {
            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);

            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        void EffectFinished()
        {

        }
    }
}
