using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode = null;

        void Awake()
        {
            currentNode = currentDialogue.GetRootNode();
        }

        public string GetText()
        {
            if (currentNode == null) return string.Empty;
            return currentNode.Text;
        }

        public void Next()
        {
            DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
            int randomIndex = Random.Range(0, children.Length);
            currentNode = children[randomIndex];
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }
    }
}
