using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        QuestList questList;
        
        void Start()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.OnQuestListUpdate += UpdateUI;
            UpdateUI();
        }

        void UpdateUI()
        {
            foreach (QuestItemUI item in GetComponentsInChildren<QuestItemUI>())
            {
                Destroy(item.gameObject);
            }
            foreach (QuestStatus status in questList.GetStatuses())
            {
                QuestItemUI questItem = Instantiate(questPrefab, transform);
                questItem.Setup(status);
            }
        }
    }
}
