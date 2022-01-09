using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking = false;
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(10, 10, 200, 100);
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;
        [SerializeField] Condition condition;

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

        public bool IsPlayerSpeaking { 
            get => isPlayerSpeaking;
#if UNITY_EDITOR
            set
            {
                Undo.RecordObject(this, "Change Dialogue Speaker");
                isPlayerSpeaking = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public string OnEnterAction => onEnterAction;
        public string OnExitAction => onExitAction;

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

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return condition.Check(evaluators);
        }
#endif

    }
}
