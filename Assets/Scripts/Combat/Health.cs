using UnityEngine;
using System.Collections;

namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;
        public bool IsDead => isDead;

        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(0, healthPoints - damage);

            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("die");
        }
    }
}
