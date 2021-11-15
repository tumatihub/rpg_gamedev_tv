using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]  
public class Mover : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] Camera mainCamera;

    NavMeshAgent navMeshAgent = null;
    Ray lastRay;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        }
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        navMeshAgent.destination = target.position;
    }
}
