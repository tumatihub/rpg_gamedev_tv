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

        PlayerConversant playerConversant;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next);
            UpdateUI();
        }

        void UpdateUI()
        {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
        }

        void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }
    }
}
