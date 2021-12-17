using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    [RequireComponent(typeof(Text))]
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health health;
        Fighter fighter;
        Text textDisplay;

        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            textDisplay = GetComponent<Text>();
        }

        void Update()
        {
            health = fighter.GetTarget();
            if (health == null)
            {
                textDisplay.text = "N/A";
                return;
            }

            textDisplay.text = $"{health.HealthPoints:0}/{health.GetMaxHealthPoints():0}";
        }
    }
}
