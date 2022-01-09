using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI rewardsText;

        public void Setup(QuestStatus status)
        {
            title.text = status.Quest.GetTitle();

            foreach (Transform child in objectiveContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Quest.Objective objective in status.Quest.GetObjectives())
            {
                GameObject prefab = objectivePrefab;
                if (!status.IsObjectiveCompleted(objective.reference))
                {
                    prefab = objectiveIncompletePrefab;
                }
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
                
            }

            rewardsText.text = GetRewardText(status.Quest);
        }

        string GetRewardText(Quest quest)
        {
            string rewardsText = "";
            foreach (Quest.Reward reward in quest.GetRewards())
            {
                if (rewardsText != "") rewardsText += ", ";
                if (reward.number > 1) rewardsText += reward.number.ToString() + " ";
                rewardsText += reward.item.GetDisplayName();
            }
            if (rewardsText == "") rewardsText = "No reward";
            rewardsText += ".";
            return rewardsText;
        }
    }
}
