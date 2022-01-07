using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest")]
    public class Quest : ScriptableObject
    {
        [SerializeField] string[] objectives;

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return objectives.Length;
        }

        public IEnumerable<string> GetObjectives()
        {
            return objectives;
        }
    }
}
