using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action OnQuestListUpdate;

        public void AddQuest(Quest quest)
        {
            if (QuestAlreadyExists(quest)) return;
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);

            OnQuestListUpdate?.Invoke();
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        bool QuestAlreadyExists(Quest quest)
        {
            foreach(QuestStatus status in statuses)
            {
                if (status.Quest == quest) return true;
            }
            return false;
        }
    }
}
