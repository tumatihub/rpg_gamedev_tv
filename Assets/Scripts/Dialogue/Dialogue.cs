using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        const float NEW_NODE_HORIZONTAL_OFFSET = 20f;

        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        void Awake()
        {
            if (nodes.Count == 0)
            {
                CreateNode(null);
            }

            OnValidate();
        }
#endif

        void OnValidate()
        {
            nodeLookup.Clear();
            
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");

            if (parent != null)
            {
                newNode.rect.x = parent.rect.xMax + NEW_NODE_HORIZONTAL_OFFSET;
                newNode.rect.y = parent.rect.y;
                parent.children.Add(newNode.name);
            }
            nodes.Add(newNode);

            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);

            OnValidate();
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.children.Remove(nodeToDelete.name);
            }
        }
    }
}
