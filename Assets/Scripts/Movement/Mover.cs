using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]  
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Health))]
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;
        Animator animator;
        ActionScheduler actionScheduler;
        Health health;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead;

            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void StartMovingAction(Vector3 destination)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}
