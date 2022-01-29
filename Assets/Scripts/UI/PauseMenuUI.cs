using UnityEngine;
using System.Collections;
using RPG.Control;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        PlayerController playerController;

        void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        void OnEnable()
        {
            Time.timeScale = 0;
            playerController.enabled = false;
        }

        void OnDisable()
        {
            Time.timeScale = 1;
            playerController.enabled = true;
        }
    }
}