using UnityEngine;
using System.Collections;
using GameDevTV.Inventories;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "RPG/Abilities/Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float cooldownTime = 0f;
        [SerializeField] float manaCost = 0;

        public override bool Use(GameObject user)
        {
            Mana mana = user.GetComponent<Mana>();
            if (mana.GetMana() < manaCost) return false;

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0)
            {
                return false;
            }

            AbilityData data = new AbilityData(user);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, () =>
            {
                TargetAquired(data);
            });
            return true;
        }

        void TargetAquired(AbilityData data)
        {
            if (data.IsCancelled()) return;

            Mana mana = data.GetUser().GetComponent<Mana>();
            if (!mana.UseMana(manaCost)) return;

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
