using UnityEngine;
using System.Collections;
using GameDevTV.Utils;
using RPG.Stats;

namespace RPG.Attributes
{
    [RequireComponent(typeof(BaseStats))]
    public class Mana : MonoBehaviour
    {
        LazyValue<float> mana;

        void Awake()
        {
            mana = new LazyValue<float>(GetMaxMana);
        }

        void Update()
        {
            if (mana.value < GetMaxMana())
            {
                mana.value += GetRegenRate() * Time.deltaTime;
                if (mana.value > GetMaxMana())
                {
                    mana.value = GetMaxMana();
                }
            }
        }

        public float GetMana()
        {
            return mana.value;
        }

        public float GetMaxMana()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public float GetRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate);
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > mana.value)
            {
                return false;
            }
            mana.value -= manaToUse;
            return true;
        }
    }
}
