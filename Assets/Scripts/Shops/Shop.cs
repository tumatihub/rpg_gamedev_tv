using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Shops
{
    public partial class Shop : MonoBehaviour, IRaycastable
    {

        [SerializeField] string shopName;

        public string ShopName => shopName;

        public event Action OnChange;

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            yield return new ShopItem(InventoryItem.GetFromID("fba84bb6-6d2c-419c-8b9c-f7a27e6e84b0"),
                10, 10.0f, 0);
            yield return new ShopItem(InventoryItem.GetFromID("46e12bcc-4fff-4c45-a07c-f52bc712bca5"),
                10, 1000.99f, 0);
            yield return new ShopItem(InventoryItem.GetFromID("b6182cb1-2ec0-45ba-a72c-51dcc068e2a8"),
                10, 20.0f, 0);
        }
        public void SelectFilter(ItemCategory category) { }
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) { }
        public bool IsBuyingMode() { return true; }
        public bool CanTransact() { return true; }
        public void ConfirmTransaction() { }
        public float TransactionTotal() { return 0; }
        public void AddToTransaction(InventoryItem item, int quantity) { }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }
    }
}
