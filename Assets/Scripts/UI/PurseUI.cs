using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Inventories;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField;

        Purse playerPurse = null;

        void Start()
        {
            playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();
            if (playerPurse != null) playerPurse.OnChange += RefreshUI;
            RefreshUI();
        }

        void RefreshUI()
        {
            balanceField.text = $"${playerPurse.Balance:N2}";
        }
    }
}
