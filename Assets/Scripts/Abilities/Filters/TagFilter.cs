using UnityEngine;
using System.Collections.Generic;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "Tag Filter", menuName = "RPG/Abilities/Filters/Tag")]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] string tagToFilter;

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var objectToFilter in objectsToFilter)
            {
                if (objectToFilter.CompareTag(tagToFilter))
                {
                    yield return objectToFilter;
                }
            }
        }
    }
}
