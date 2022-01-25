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

        public override void Use(GameObject user)
        {
            targetingStrategy.StartTargeting(user, TargetAquired);
        }

        void TargetAquired(IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                Debug.Log($"Target Aquired: {target.name}");
            }
        }
    }
}
