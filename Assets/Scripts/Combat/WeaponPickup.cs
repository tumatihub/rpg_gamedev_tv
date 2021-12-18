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
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Fighter fighter = other.GetComponent<Fighter>();
                Pickup(fighter);
            }
        }

        void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
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
                Fighter fighter = callingController.GetComponent<Fighter>();
                Pickup(fighter);
            }
            return true;
        }
    }
}
