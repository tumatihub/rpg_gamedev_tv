using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]  
public class Mover : MonoBehaviour
{
    [SerializeField] Transform target = null;
    NavMeshAgent navMeshAgent = null;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        navMeshAgent.destination = target.position;
    }
}
