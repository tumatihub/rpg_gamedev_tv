using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Text))]
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        Text textDisplay;

        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            textDisplay = GetComponent<Text>();
        }

        void Update()
        {
            textDisplay.text = $"{experience.ExperiencePoints}";
        }
    }
}
