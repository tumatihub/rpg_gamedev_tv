using GameDevTV.Inventories;
using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Quests
{
    [RequireComponent(typeof(ItemDropper))]
    [RequireComponent(typeof(Inventory))]
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action OnQuestListUpdate;
        ItemDropper itemDropper;

        Inventory inventory;

        void Awake()
        {
            inventory = GetComponent<Inventory>();
            itemDropper = GetComponent<ItemDropper>();
        }

        void Update()
        {
            CompleteObjectivesByPredicates();
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);

            OnQuestListUpdate?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            if (status == null) return;

            status.CompleteObjective(objective);
            if (status.IsComplete())
            {
                GiveReward(quest);
            }

            OnQuestListUpdate?.Invoke();
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        bool HasQuest(Quest quest)
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
        
        void CompleteObjectivesByPredicates()
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.IsComplete()) continue;
                Quest quest = status.Quest;
                foreach (var objective in quest.GetObjectives())
                {
                    if (status.IsObjectiveCompleted(objective.reference)) continue;
                    if (!objective.usesCondition) continue;

                    if (objective.completionCondition.Check(GetComponents<IPredicateEvaluator>()))
                    {
                        CompleteObjective(quest, objective.reference);
                    }
                }
            }
        }

        void GiveReward(Quest quest)
        {
            foreach (Quest.Reward reward in quest.GetRewards())
            {
                bool success = inventory.AddToFirstEmptySlot(reward.item, reward.number);
                if (!success) itemDropper.DropItem(reward.item, reward.number);
            }
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

        public bool? Evaluate(string predicate, string[] parameters)
        {
            Quest quest;
            switch (predicate)
            {
                case "HasQuest":
                    quest = Quest.GetByName(parameters[0]);
                    return HasQuest(quest);
                case "CompletedQuest":
                    quest = Quest.GetByName(parameters[0]);
                    QuestStatus status = GetQuestStatus(quest);
                    return status != null && status.IsComplete();
                default:
                    return null;
            }
        }
    }
}
