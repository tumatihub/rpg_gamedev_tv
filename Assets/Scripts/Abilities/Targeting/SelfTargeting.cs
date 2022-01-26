using UnityEngine;
using System.Collections;
using System;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Self Targeting", menuName = "RPG/Abilities/Targeting/Self")]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[] { data.GetUser() });
            data.SetTargetedPoint(data.GetUser().transform.position);
            finished();
        }
    }
}
