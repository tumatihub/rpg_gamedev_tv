using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text textDisplay;

        public void SetDamageValue(float damage)
        {
            textDisplay.text = $"{damage:0}";
        }
    }
}
