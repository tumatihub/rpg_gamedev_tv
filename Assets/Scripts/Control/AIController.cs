using UnityEngine;
using System.Collections;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        void Update()
        {
            if (DistanceToPlayer() <= chaseDistance)
            {
                print($"Start Chase: {transform.name}");
            }
        }

        float DistanceToPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return Vector3.Distance(player.transform.position, transform.position);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
