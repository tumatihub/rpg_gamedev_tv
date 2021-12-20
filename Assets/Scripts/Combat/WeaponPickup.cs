using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Collider))]
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] float healthToRestore = 0;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        void Pickup(GameObject subject)
        {
            if (weapon != null)
            {
                Fighter fighter = subject.GetComponent<Fighter>();
                fighter.EquipWeapon(weapon);
            }
            if (healthToRestore > 0)
            {
                Health health = subject.GetComponent<Health>();
                health.Heal(healthToRestore);
            }
            StartCoroutine(HideForSeconds(respawnTime));
        }

        IEnumerator HideForSeconds(float seconds)
        {
            HidePickup();
            yield return new WaitForSeconds(seconds);
            ShowPickup();
        }
        void HidePickup()
        {
            GetComponent<Collider>().enabled = false;
            foreach (Transform child in GetComponentInChildren<Transform>())
            {
                child.gameObject.SetActive(false);
            }
        }

        void ShowPickup()
        {
            GetComponent<Collider>().enabled = true;
            foreach (Transform child in GetComponentInChildren<Transform>())
            {
                child.gameObject.SetActive(true);
            }
        }

        public bool HandleRaycastable(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
