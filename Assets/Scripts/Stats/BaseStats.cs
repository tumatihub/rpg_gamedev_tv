using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;
        [SerializeField] GameObject levelUpParticleEffect = null;
 
        int currentLevel = 0;

        public event Action onLevelUp;

        Experience experience;

        void Awake()
        {
            experience = GetComponent<Experience>();
        }

        void Start()
        {
            currentLevel = CalculateLevel();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel()) + GetAdditiveModifiers(stat);
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        int CalculateLevel()
        {
            if (experience == null) return startingLevel;

            float currentXP = experience.ExperiencePoints;
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        float GetAdditiveModifiers(Stat stat)
        {
            float total = 0;
            foreach(IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
    }
}
