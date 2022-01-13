using RPG.Shops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalField;
        [SerializeField] Button confirmButton;

        Shopper shopper;
        Shop currentShop;
        Color originalTotalTextColor;

        void Start()
        {
            originalTotalTextColor = totalField.color;
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.OnActiveShopChange += ShopChanged;
            confirmButton.onClick.AddListener(ConfirmTransaction);
            ShopChanged();
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        void ShopChanged()
        {
            if (currentShop != null)
            {
                currentShop.OnChange -= RefreshUI;
            }

            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
            if (currentShop == null) return;

            shopName.text = currentShop.ShopName;

            currentShop.OnChange += RefreshUI;

            RefreshUI();
        }

        void RefreshUI()
        {
            foreach (Transform row in listRoot)
            {
                Destroy(row.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate(rowPrefab, listRoot);
                row.Setup(currentShop, item);
            }

            totalField.text = $"Total: ${currentShop.TransactionTotal():N2}";
            totalField.color = currentShop.HasSufficientFunds() ? originalTotalTextColor : Color.red;
            confirmButton.interactable = currentShop.CanTransact();
        }
    }
}
