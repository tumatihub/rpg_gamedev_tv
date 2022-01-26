using UnityEngine;
using System.Collections;
using System;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "RPG/Abilities/Effects/Spawn Projectile")]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] float damage;
        [SerializeField] bool isRightHand = true;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Fighter fighter = data.GetUser().GetComponent<Fighter>();
            Vector3 spawnPosition = fighter.GetHandTransform(isRightHand).position;

            foreach (var target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health == null) continue;

                Projectile projectile = Instantiate(projectileToSpawn);
                projectile.transform.position = spawnPosition;
                projectile.SetTarget(health, data.GetUser(), damage);
            }
        }
    }
}
