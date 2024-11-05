using System.Collections;
using System.Collections.Generic;
using TexAnim.Windows;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TexAnim
{
    public class SelectionArea : VisualElement
    {
        private TexAnimAnimatorEditorWindow window;
        private TexAnimGraphView graphView;
        private VisualElement selectionArea;
        private Vector2 startPos;
        private bool isSelecting;

        public SelectionArea(TexAnimGraphView element, TexAnimAnimatorEditorWindow editorwindow)
        {
            graphView = element;
            window = editorwindow;
        }


        public void OnEnable()
        {
            // Create a root VisualElement
            var root = graphView;

            // Create the selection area VisualElement
            selectionArea = new VisualElement();
            selectionArea.style.backgroundColor = new Color(0, 0.3f, 1.0f, 0.3f); // Blue with 30% opacity
            selectionArea.style.position = Position.Absolute; // Position it absolutely within the container
            selectionArea.style.left = 0; // Set initial position
            selectionArea.style.top = 0; // Set initial position
            root.Add(selectionArea); // Add it to the root

            // Subscribe to mouse events
            root.RegisterCallback<MouseDownEvent>(OnMouseDown);
            root.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            root.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0) // Left mouse button
            {
                isSelecting = true;
                startPos = GetLocalMousePosition(evt.mousePosition);
                selectionArea.style.display = DisplayStyle.Flex; // Show the selection area

                selectionArea.style.left = Mathf.Min(GetLocalMousePosition(evt.mousePosition).x);
                selectionArea.style.top = Mathf.Min(GetLocalMousePosition(evt.mousePosition).y, startPos.y);
                selectionArea.style.top = GetLocalMousePosition(evt.mousePosition).y;

            }
        }

        void OnMouseMove(MouseMoveEvent evt)
        {
            if (isSelecting)
            {
                // Calculate width and height of the selection area
                float width = Mathf.Abs((GetLocalMousePosition(evt.mousePosition).x) - startPos.x);
                float height = Mathf.Abs(GetLocalMousePosition(evt.mousePosition).y - startPos.y);

                // Update position and size of the selection area
                selectionArea.style.left = Mathf.Min(GetLocalMousePosition(evt.mousePosition).x, startPos.x);
                selectionArea.style.top = Mathf.Min(GetLocalMousePosition(evt.mousePosition).y, startPos.y);

                selectionArea.style.width = width;
                selectionArea.style.height = height;
            }
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button == 0) // Left mouse button
            {
                isSelecting = false;
                selectionArea.style.display = DisplayStyle.None; // Hide the selection area

                //Reset everything
                startPos = Vector2.zero;

                // Update position and size of the selection area
                selectionArea.style.left = Mathf.Min(GetLocalMousePosition(evt.mousePosition).x, startPos.x);
                selectionArea.style.top = Mathf.Min(GetLocalMousePosition(evt.mousePosition).y, startPos.y);
                selectionArea.style.width = 0;
                selectionArea.style.height = 0;
            }
        }


        public Vector2 GetLocalMousePosition(Vector2 mousePosition)
        {
            Vector2 worldMousePosition = mousePosition;
            Vector2 localMousePosition = graphView.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }
    }
}
