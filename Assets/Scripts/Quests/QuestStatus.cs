using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord statusRecord = objectState as QuestStatusRecord;
            if (statusRecord == null) return;

            quest = Quest.GetByName(statusRecord.questName);
            completedObjectives = statusRecord.completedObjectives;
        }

        public bool IsComplete()
        {
            foreach (Quest.Objective objective in quest.GetObjectives())
            {
                if (!completedObjectives.Contains(objective.reference)) return false;
            }
            return true;
        }

        public Quest Quest  => quest;

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveCompleted(string objective)
        {
            return completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective)) completedObjectives.Add(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;
        }
    }
}
