﻿using UnityEngine;
using System.Collections;
using RPG.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] Transform contentRoot;
        [SerializeField] GameObject buttonPrefab;

        void OnEnable()
        {
            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }

            var savingWrapper = FindObjectOfType<SavingWrapper>();

            foreach (string save in savingWrapper.ListSaves())
            {
                GameObject button = Instantiate(buttonPrefab, contentRoot);
                var buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = save;
                Button buttonComp = button.GetComponent<Button>();
                buttonComp.onClick.AddListener(() => savingWrapper.LoadGame(save));
            }
        }
    }
}