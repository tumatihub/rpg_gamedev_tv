using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour
    {
        [SerializeField] float startingBalance = 400f;

        float balance = 0;

        public event Action OnChange;

        public float Balance => balance;

        void Awake()
        {
            balance = startingBalance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            OnChange?.Invoke();
        }
    }
}
