using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Fighter fighter = other.GetComponent<Fighter>();
                fighter.EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
