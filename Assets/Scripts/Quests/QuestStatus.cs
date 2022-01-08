using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives;

        public Quest Quest  => quest;

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveCompleted(string objective)
        {
            return completedObjectives.Contains(objective);
        }
    }
}
