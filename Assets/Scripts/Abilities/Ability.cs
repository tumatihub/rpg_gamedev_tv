using UnityEngine;
using System.Collections;
using GameDevTV.Inventories;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "RPG/Abilities/Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;

        public override void Use(GameObject user)
        {
            targetingStrategy.StartTargeting();
        }
    }
}
