using UnityEngine;
using System.Collections;
using System;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "RPG/Abilities/Effects/Trigger Animation")]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] string animationTrigger;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Animator animation = data.GetUser().GetComponent<Animator>();
            animation.SetTrigger(animationTrigger);
            finished();
        }
    }
}
