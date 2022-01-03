using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        const int ATTEMPTS = 30;

        [Tooltip("How far can the pickups be scattered from the dropper")]
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] InventoryItem[] dropLibrary;
        [SerializeField] int numberOfDrops = 2;

        public void RandomDrop()
        {
            for (int i = 0; i < numberOfDrops; i++)
            {
                var item = dropLibrary[Random.Range(0, dropLibrary.Length)];
                DropItem(item, 1);
            }
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, .1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }
    }
}
