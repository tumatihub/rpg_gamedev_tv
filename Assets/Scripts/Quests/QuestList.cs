using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
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

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            if (status == null) return;

            status.CompleteObjective(objective);

            OnQuestListUpdate?.Invoke();
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        bool QuestAlreadyExists(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.Quest == quest) return status;
            }
            return null;
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }
    }
}
