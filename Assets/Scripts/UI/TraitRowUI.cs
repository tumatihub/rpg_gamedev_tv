using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] Trait trait;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;

        TraitStore playerTraitStore = null;

        void Start()
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(1));
        }

        void Update()
        {
            minusButton.interactable = playerTraitStore.CanAssignPoints(trait, -1);
            plusButton.interactable = playerTraitStore.CanAssignPoints(trait, 1);
            
            valueText.text = playerTraitStore.GetPoints(trait).ToString();
        }

        public void Allocate(int points)
        {
            playerTraitStore.AssignPoints(trait, points);
        }
    }
}