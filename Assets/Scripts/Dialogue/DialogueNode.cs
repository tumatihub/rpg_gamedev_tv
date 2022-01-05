using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(10, 10, 200, 100);

        public string Text { 
            get => text;
#if UNITY_EDITOR
            set
            {
                if (value != text)
                {
                    Undo.RecordObject(this, "Update Dialogue Text");
                    text = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

        public List<string> Children { 
            get => children;
            set
            {
                children = value;
            }
        }

        public Rect Rect { 
            get => rect;
        }

#if UNITY_EDITOR
        public void SetRectPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            this.rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif

    }
}
