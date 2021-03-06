using UnityEngine;
using System.Collections;
using GameDevTV.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    [RequireComponent(typeof(BaseStats))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ActionScheduler))]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent<float> takeDamage;
        public UnityEvent onDie;

        LazyValue<float> healthPoints;

        bool wasDeadLastFrame = false;
        public bool IsDead => healthPoints.value <= 0;
        public float HealthPoints => healthPoints.value;

        Animator animator;
        BaseStats baseStats;

        void Awake()
        {
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        void Start()
        {
            healthPoints.ForceInit();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            UpdateState();
        }

        float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        void OnEnable()
        {
            baseStats.onLevelUp += RegenerateHealth;
        }

        void OnDisable()
        {
            baseStats.onLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(0, healthPoints.value - damage);
            takeDamage.Invoke(damage);

            if (IsDead)
            {
                onDie.Invoke();
                AwardExperience(instigator);
            }
            UpdateState();
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return (healthPoints.value / baseStats.GetStat(Stat.Health));
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void UpdateState()
        {
            if (!wasDeadLastFrame && IsDead)
            {
                animator.SetTrigger("die");
                GetComponent<Collider>().enabled = false;
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (wasDeadLastFrame && !IsDead)
            {
                animator.Rebind();
                GetComponent<Collider>().enabled = true;
            }

            wasDeadLastFrame = IsDead;
        }

        void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        void RegenerateHealth()
        {
            healthPoints.value = baseStats.GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            UpdateState();
        }
    }
}
