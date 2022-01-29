using UnityEngine;
using System.Collections;

namespace RPG.UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] Transform contentRoot;
        [SerializeField] GameObject buttonPrefab;

        void OnEnable()
        {
            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }

            Instantiate(buttonPrefab, contentRoot);
            Instantiate(buttonPrefab, contentRoot);
            Instantiate(buttonPrefab, contentRoot);
            Instantiate(buttonPrefab, contentRoot);
        }
    }
}