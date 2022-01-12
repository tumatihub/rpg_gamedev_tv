using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Shops
{ 
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public int Availability => availability;
        public float Price => price;
        public int QuantityInTransaction => quantityInTransaction;
        public InventoryItem Item => item;

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }
    }
}
