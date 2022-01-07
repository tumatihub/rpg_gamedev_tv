using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string conversantName = string.Empty;

        public string ConversantName  => conversantName;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null) return false;

            if (Input.GetMouseButtonDown(0))
            {
                PlayerConversant playerConversant = callingController.GetComponent<PlayerConversant>();
                playerConversant.StartDialogue(this, dialogue);
            }
            return true;
        }
    }
}
