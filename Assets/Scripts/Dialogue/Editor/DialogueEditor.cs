using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        const float BACKGROUND_SIZE = 50f;
        const float MIN_CANVAS_WIDHT = 1000f;
        const float MIN_CANVAS_HEIGHT = 500f;

        Dialogue selectedDialogue = null;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        float xMaxWindow = MIN_CANVAS_WIDHT;
        float yMaxWindow = MIN_CANVAS_HEIGHT;
        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor", true);
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        void OnSelectionChanged()
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if (dialogue != null)
            {
                selectedDialogue = dialogue;
                ResetScrollPosition();
                UpdateScrollMaxSize();
                Repaint();
            }
        }

        void ResetScrollPosition()
        {
            scrollPosition = Vector2.zero;
        }

        void UpdateScrollMaxSize()
        {
            float xMax = MIN_CANVAS_WIDHT;
            float yMax = MIN_CANVAS_HEIGHT;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                xMax = Mathf.Max(node.rect.xMax, xMax);
                yMax = Mathf.Max(node.rect.yMax, yMax);
            }

            xMaxWindow = xMax;
            yMaxWindow = yMax;
        }

        void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(xMaxWindow, yMaxWindow);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, xMaxWindow/BACKGROUND_SIZE, yMaxWindow/BACKGROUND_SIZE);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if (deletingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Removed Dialogue Node");
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }

        void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                UpdateScrollSize(draggingNode.rect.xMax, draggingNode.rect.yMax);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        void UpdateScrollSize(float xMax, float yMax)
        {
            xMaxWindow = Mathf.Max(xMax, xMaxWindow);
            yMaxWindow = Mathf.Max(yMax, yMaxWindow);
        }

        void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.text = newText;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("-"))
            {
                deletingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (node == linkingParentNode)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.children.Contains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    Undo.RecordObject(selectedDialogue, "Remove Dialogue Link");
                    linkingParentNode.children.Remove(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    Undo.RecordObject(selectedDialogue, "Add Dialogue Link");
                    linkingParentNode.children.Add(node.name);
                    linkingParentNode = null;
                }
            }
        }

        void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(
                node.rect.xMax,
                node.rect.center.y
            );
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(
                    childNode.rect.xMin,
                    childNode.rect.center.y
                );

                float bezierOffset = Mathf.Abs(childNode.rect.xMin - node.rect.xMax)/2f;

                Handles.DrawBezier(startPosition, endPosition, startPosition + (Vector3.right * bezierOffset), endPosition + (Vector3.left * bezierOffset), Color.white, null, 4f);
            }
        }

        DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(point)) foundNode = node;
            }
            return foundNode;
        }
    }
}
