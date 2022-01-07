using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;

        Quest quest;

        public Quest Quest => quest;

        public void Setup(Quest quest)
        {
            title.text = quest.GetTitle();
            progress.text = $"0/{quest.GetObjectiveCount()}";
            this.quest = quest;
        }
    }
}
