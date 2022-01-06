using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;
using System;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;

        PlayerConversant playerConversant;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.OnConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(Next);
            UpdateUI();
        }

        void UpdateUI()
        {
            if (!playerConversant.IsActive) return;

            AIResponse.SetActive(!playerConversant.IsChoosing);
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing);
            if (playerConversant.IsChoosing)
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (DialogueNode choiceNode in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                TextMeshProUGUI choiceButtonText = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                choiceButtonText.text = choiceNode.Text;
                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => {
                    playerConversant.SelectChoice(choiceNode);
                });
            }
        }

        void Next()
        {
            playerConversant.Next();
        }
    }
}
