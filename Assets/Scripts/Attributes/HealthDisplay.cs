using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Text))]
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text textDisplay;

        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            textDisplay = GetComponent<Text>();
        }

        void Update()
        {
            textDisplay.text = $"{health.HealthPoints:0}/{health.GetMaxHealthPoints():0}";
        }
    }
}
