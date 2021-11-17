using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]  
    [RequireComponent(typeof(Animator))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Fighter fighter;


        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
        }
        void Update()
        {
            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void StartMovingAction(Vector3 destination)
        {
            fighter.Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
        }
    }
}
