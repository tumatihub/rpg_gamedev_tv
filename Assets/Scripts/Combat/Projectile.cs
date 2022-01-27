using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onHit;
        
        Health target;
        Vector3 targetPoint;
        GameObject instigator;
        float damage = 0;

        void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target != null && isHoming && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            Health target = other.GetComponent<Health>();
            if (this.target != null && target != this.target) return;
            if (target == null || target.IsDead) return;
            if (other.gameObject == instigator) return;

            target.TakeDamage(instigator, damage);

            speed = 0;

            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget(GameObject instigator, float damage, Health target=null, Vector3 targetPoint=default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
    }
}
