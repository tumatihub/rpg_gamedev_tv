using RPG.Shops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;

        Shopper shopper;
        Shop currentShop;

        void Start()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;
            shopper.OnActiveShopChange += ShopChanged;

            ShopChanged();
        }

        void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
            if (currentShop == null) return;

            shopName.text = currentShop.ShopName;
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }
    }
}
