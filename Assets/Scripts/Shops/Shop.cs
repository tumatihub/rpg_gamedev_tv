using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Inventories;

namespace RPG.Shops
{
    public partial class Shop : MonoBehaviour, IRaycastable
    {
        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0,100)]
            public float buyingDiscountPercentage;
        }

        [SerializeField] string shopName;
        [SerializeField] StockItemConfig[] stockConfig;

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();
        Shopper currentShopper;

        public string ShopName => shopName;

        public event Action OnChange;

        void Awake()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                stock[config.item] = config.initialStock;
            }
        }

        public void SetShopper(Shopper shopper)
        {
            this.currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return GetAllItems();
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
                int quantityInTransaction = 0;
                transaction.TryGetValue(config.item, out quantityInTransaction);
                yield return new ShopItem(config.item, stock[config.item], price, quantityInTransaction);
            }
        }

        public void SelectFilter(ItemCategory category) { }
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) { }
        public bool IsBuyingMode() { return true; }
        public bool CanTransact()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSufficientFunds()) return false;
            if (!HasInventorySpace()) return false;
            return true;
        }

        public void ConfirmTransaction()
        {
            Purse purse = currentShopper.GetComponent<Purse>();
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null || purse == null) return;

            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.Item;
                int quantity = shopItem.QuantityInTransaction;
                float price = item.GetPrice();
                
                for (int i = 0; i < quantity; i++)
                {
                    if (purse.Balance < price) break;
                    
                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
                    if (success)
                    {
                        AddToTransaction(item, -1);
                        stock[item]--;
                        purse.UpdateBalance(-price);
                    }
                }
            }

            OnChange?.Invoke();
        }

        public float TransactionTotal()
        {
            float total = 0;
            foreach (ShopItem item in GetAllItems())
            {
                total += item.Price * item.QuantityInTransaction;
            }
            return total;
        }
        
        public void AddToTransaction(InventoryItem item, int quantity)
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            if (transaction[item] + quantity > stock[item])
            {
                transaction[item] = stock[item];
            }
            else
            {
                transaction[item] += quantity;
            }

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            OnChange?.Invoke();
        }

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

        public bool HasSufficientFunds()
        {
            Purse purse = currentShopper.GetComponent<Purse>();
            if (purse == null) return false;
            return purse.Balance >= TransactionTotal();
        }

        public bool HasInventorySpace()
        {
            Inventory inventory = currentShopper.GetComponent<Inventory>();
            if (inventory == null) return false;

            List<InventoryItem> flatItems = new List<InventoryItem>();
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.Item;
                int quantity = shopItem.QuantityInTransaction;
                for (int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            }
            return inventory.HasSpaceFor(flatItems);
        }

        public bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }
    }
}