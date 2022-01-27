using UnityEngine;
using System.Collections;
using GameDevTV.Utils;
using RPG.SceneManagement;
using System;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        LazyValue<SavingWrapper> savingWrapper;

        void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}