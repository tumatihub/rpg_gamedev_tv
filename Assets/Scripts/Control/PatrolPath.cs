using UnityEngine;
using System.Collections;
using System;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;

        void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWaypointIndex(i);
                Gizmos.color = GetWaypointColor(i);
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        Color GetWaypointColor(int waypointIndex)
        {
            return (waypointIndex == 0) ? Color.red : Color.white;
        }

        public int GetNextWaypointIndex(int currentIndex)
        {
            int nextIndex = currentIndex + 1;
            return (nextIndex >= transform.childCount) ? 0 : nextIndex;
        }

        public Vector3 GetWaypoint(int childIndex)
        {
            return transform.GetChild(childIndex).transform.position;
        }
    }
}
