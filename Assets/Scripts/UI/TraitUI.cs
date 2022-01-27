using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using RPG.Stats;

namespace RPG.UI
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI unassignedPointsText;
        [SerializeField] Button commitButton;

        TraitStore playerTraitStore = null;

        void Start()
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            commitButton.onClick.AddListener(playerTraitStore.Commit);
        }

        void Update()
        {
            unassignedPointsText.text = playerTraitStore.GetUnassignedPoints().ToString();
        }
    }
}