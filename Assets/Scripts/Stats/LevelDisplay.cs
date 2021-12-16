using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    [RequireComponent(typeof(Text))]
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        Text textDisplay;

        void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            textDisplay = GetComponent<Text>();
        }

        void Update()
        {
            textDisplay.text = $"{baseStats.GetLevel()}";
        }
    }
}
