using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour
    {
        [SerializeField] float startingBalance = 400f;

        float balance = 0;

        public float Balance => balance;

        void Awake()
        {
            balance = startingBalance;
            print($"Balance: {balance:N2}");
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            print($"Balance: {balance:N2}");
        }
    }
}
