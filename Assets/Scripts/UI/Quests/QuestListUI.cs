using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] Quest[] tempQuests;
        [SerializeField] QuestItemUI questPrefab;
        
        void Start()
        {
            foreach (QuestItemUI item in GetComponentsInChildren<QuestItemUI>())
            {
                Destroy(item.gameObject);
            }
            foreach (Quest quest in tempQuests)
            {
                QuestItemUI questItem = Instantiate(questPrefab, transform);
                questItem.Setup(quest);
            }
        }
    }
}
