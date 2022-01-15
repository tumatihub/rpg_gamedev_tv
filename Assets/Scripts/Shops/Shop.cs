using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Inventories;
using RPG.Stats;

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
            public int levelToUnlock = 0;
        }

        [SerializeField] string shopName;
        [SerializeField] StockItemConfig[] stockConfig;
        [Range(0, 100)]
        [SerializeField] float sellingPercentage = 80f;

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();
        Shopper currentShopper;
        bool isBuyingMode = true;
        ItemCategory currentFilter = ItemCategory.None;

        public string ShopName => shopName;
        public bool IsBuyingMode => isBuyingMode;

        public event Action OnChange;

        public void SetShopper(Shopper shopper)
        {
            this.currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (ShopItem shopItem in GetAllItems())
            {
                ItemCategory itemCategory = shopItem.Item.GetCategory();
                if (currentFilter == ItemCategory.None || itemCategory == currentFilter)
                {
                    yield return shopItem;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            Dictionary<InventoryItem, float> prices = GetPrices();
            Dictionary<InventoryItem, int> availabilities = GetAvailabilities();
            foreach (InventoryItem item in availabilities.Keys)
            {
                if (availabilities[item] <= 0) continue;

                float price = prices[item];
                int quantityInTransaction = 0;
                transaction.TryGetValue(item, out quantityInTransaction);
                yield return new ShopItem(item, availabilities[item], price, quantityInTransaction);
            }
        }

        public void SelectFilter(ItemCategory category)
        {
            currentFilter = category;
            OnChange?.Invoke();
        }

        public ItemCategory GetFilter()
        {
            return currentFilter;
        }
        
        public void SelectMode(bool isBuying)
        {
            isBuyingMode = isBuying;
            OnChange?.Invoke();
        }

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
                float price = shopItem.Price;
                
                for (int i = 0; i < quantity; i++)
                {
                    if (isBuyingMode)
                    {
                        BuyItem(shopperInventory, purse, item, price);
                    }
                    else
                    {
                        SellItem(shopperInventory, purse, item, price);
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

            var availabilities = GetAvailabilities();
            int availability = availabilities[item];
            if (transaction[item] + quantity > availability)
            {
                transaction[item] = availability;
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
            if (!isBuyingMode) return true;

            Purse purse = currentShopper.GetComponent<Purse>();
            if (purse == null) return false;
            return purse.Balance >= TransactionTotal();
        }

        public bool HasInventorySpace()
        {
            if (!isBuyingMode) return true;

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

        int CountItemsInInventory(InventoryItem item)
        {
            Inventory inventory = currentShopper.GetComponent<Inventory>();
            if (inventory == null) return 0;

            int count = 0;
            for (int i = 0; i < inventory.GetSize(); i++)
            {
                InventoryItem slotItem = inventory.GetItemInSlot(i);
                if (slotItem != item) continue;

                count += inventory.GetNumberInSlot(i);
            }
            return count;
        }

        void SellItem(Inventory shopperInventory, Purse purse, InventoryItem item, float price)
        {
            int slot = FindFirstItemSlot(shopperInventory, item);
            if (slot == -1) return;
            AddToTransaction(item, -1);
            shopperInventory.RemoveFromSlot(slot, 1);
            if (!stockSold.ContainsKey(item))
            {
                stockSold[item] = 0;
            }
            stockSold[item]--;
            purse.UpdateBalance(price);
            
        }

        int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if (shopperInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }
            }
            return -1;
        }

        void BuyItem(Inventory shopperInventory, Purse purse, InventoryItem item, float price)
        {
            if (purse.Balance < price) return;

            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                AddToTransaction(item, -1);
                if (!stockSold.ContainsKey(item))
                {
                    stockSold[item] = 0;
                }
                stockSold[item]++;
                purse.UpdateBalance(-price);
            }
        }

        int GetShopperLevel()
        {
            BaseStats stats = currentShopper.GetComponent<BaseStats>();
            if (stats == null) return 0;

            return stats.GetLevel();
        }

        Dictionary<InventoryItem, int> GetAvailabilities()
        {
            Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!availabilities.ContainsKey(config.item))
                    {
                        int sold = 0;
                        stockSold.TryGetValue(config.item, out sold);
                        availabilities[config.item] = -sold;
                    }
                    availabilities[config.item] += config.initialStock;
                }
                else
                {
                    availabilities[config.item] = CountItemsInInventory(config.item);
                }
            }

            return availabilities;
        }

        Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!prices.ContainsKey(config.item))
                    {
                        prices[config.item] = config.item.GetPrice();
                    }
                    prices[config.item] *= (1 - config.buyingDiscountPercentage / 100);
                }
                else
                {
                    prices[config.item] = config.item.GetPrice() * (sellingPercentage / 100);
                }
            }

            return prices;
        }

        IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            int shopperLevel = GetShopperLevel();

            foreach (var config in stockConfig)
            {
                if (config.levelToUnlock > shopperLevel) continue;

                yield return config;
            }
        }
    }
}
