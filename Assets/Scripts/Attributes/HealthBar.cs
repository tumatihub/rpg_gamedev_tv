using UnityEngine;
using System.Collections;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health;
        [SerializeField] RectTransform foreground;
        [SerializeField] Canvas canvas;

        float fraction;

        void Update()
        {
            fraction = health.GetFraction();

            if (Mathf.Approximately(fraction, 0) || Mathf.Approximately(fraction, 1))
            {
                canvas.enabled = false;
                return;
            }

            canvas.enabled = true;
            foreground.localScale = new Vector3(
                health.GetFraction(),
                foreground.localScale.y,
                foreground.localScale.z
            );
        }
    }
}
