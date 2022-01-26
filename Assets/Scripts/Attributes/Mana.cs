using UnityEngine;
using System.Collections;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour
    {
        [SerializeField] float maxMana = 200f;
        [SerializeField] float manaRegenRate = 2;

        float mana;

        void Awake()
        {
            mana = maxMana;
        }

        void Update()
        {
            if (mana < maxMana)
            {
                mana += manaRegenRate * Time.deltaTime;
                if (mana > maxMana)
                {
                    mana = maxMana;
                }
            }
        }

        public float GetMana()
        {
            return mana;
        }

        public float GetMaxMana()
        {
            return maxMana;
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > mana)
            {
                return false;
            }
            mana -= manaToUse;
            return true;
        }
    }
}
