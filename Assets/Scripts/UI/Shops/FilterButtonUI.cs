using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameDevTV.Inventories;
using System;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {
        [SerializeField] ItemCategory category = ItemCategory.None;

        Button button;
        Shop currentShop;

        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);
        }

        public void SetShop(Shop currentShop)
        {
            this.currentShop = currentShop;
        }

        void SelectFilter()
        {
            currentShop.SelectFilter(category);
        }
    }
}
