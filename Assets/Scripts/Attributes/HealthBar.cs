using UnityEngine;
using System.Collections;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health;
        [SerializeField] RectTransform foreground;

        void Update()
        {
            foreground.localScale = new Vector3(
                health.GetFraction(),
                foreground.localScale.y,
                foreground.localScale.z
            );
        }
    }
}
