using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level) return 0;

            return levels[level - 1];
        }

        void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (var progressionClass in characterClasses)
            {
                Dictionary<Stat, float[]> stats = new Dictionary<Stat, float[]>();
                foreach (var progressionStat in progressionClass.stats)
                {
                    stats.Add(progressionStat.stat, progressionStat.levels);
                }
                lookupTable.Add(progressionClass.characterClass, stats);
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}