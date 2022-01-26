using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Text))]
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana;
        Text textDisplay;

        void Awake()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
            textDisplay = GetComponent<Text>();
        }

        void Update()
        {
            textDisplay.text = $"{mana.GetMana():0}/{mana.GetMaxMana():0}";
        }
    }
}
