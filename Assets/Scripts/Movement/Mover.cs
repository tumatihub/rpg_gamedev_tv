using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]  
[RequireComponent(typeof(Animator))]
public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Camera mainCamera;

    NavMeshAgent navMeshAgent;
    Animator animator;


    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }

        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
        animator.SetFloat("forwardSpeed", localVelocity.z);
    }

    void MoveToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            navMeshAgent.destination = hit.point;
        }
    }
}
