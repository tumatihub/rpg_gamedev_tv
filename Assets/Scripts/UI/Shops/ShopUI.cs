using RPG.Shops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalField;

        Shopper shopper;
        Shop currentShop;

        void Start()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.OnActiveShopChange += ShopChanged;
            
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
        }
    }
}
