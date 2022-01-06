using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue testDialogue;

        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        public bool IsChoosing => isChoosing;
        public bool IsActive => currentDialogue != null;

        public event Action OnConversationUpdated;

        public void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            OnConversationUpdated?.Invoke();
        }

        public void Quit()
        {
            currentDialogue = null;
            isChoosing = false;
            OnConversationUpdated?.Invoke();
        }

        public string GetText()
        {
            if (currentNode == null) return string.Empty;
            return currentNode.Text;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                OnConversationUpdated?.Invoke();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            currentNode = children[randomIndex];
            OnConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }
    }
}
