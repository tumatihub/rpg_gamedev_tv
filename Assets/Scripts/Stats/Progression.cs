using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach (var progressionClass in characterClasses)
            {
                if (progressionClass.characterClass != characterClass) continue;
                
                foreach (var progressionStat in progressionClass.stats)
                {
                    if (progressionStat.stat != stat) continue;
                    
                    return progressionStat.levels[Mathf.Min(level - 1, progressionStat.levels.Length - 1)];
                }
            }
            return 0;
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