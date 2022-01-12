using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField] Image iconField;
        [SerializeField] TextMeshProUGUI nameField;
        [SerializeField] TextMeshProUGUI availabilityField;
        [SerializeField] TextMeshProUGUI priceField;

        Shop currentShop;
        ShopItem shopItem;

        public void Setup(Shop currentShop, ShopItem item)
        {
            this.currentShop = currentShop;
            shopItem = item;
            iconField.sprite = item.GetIcon();
            nameField.text = item.GetName();
            availabilityField.text = item.Availability.ToString();
            priceField.text = $"${item.Price:N2}";
        }

        public void Add()
        {
            currentShop.AddToTransaction(shopItem.Item, 1);
        }

        public void Remove()
        {
            currentShop.AddToTransaction(shopItem.Item, -1);
        }
    }
}
