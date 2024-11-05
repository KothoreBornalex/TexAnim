using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Reflection;
using System.IO;

namespace TexAnim.Windows
{
    using Elements;
    using Enumerations;
    using TexAnim.Data.Error;
    using TexAnim.Data;
    using static ScriptableObject_AnimationsBank;
    using TexAnim.Editor.Utilities;
    using Unity.VisualScripting;

    public class TexAnimGraphView : GraphView
    {
        private List<GraphElement> _visuals = new List<GraphElement>();
        public TexAnimAnimatorEditorWindow parentWindow;
        public Label animatorNameLabel;
        private MiniMap _miniMap;
        public TexAnimGraphView(TexAnimAnimatorEditorWindow parentWindow)
        {
            this.parentWindow = parentWindow;


            AddManipulators();
            AddMiniMap();
            AddGridBackground();
            AddGraphViewHeader();

            OnElementsDeleted();

            AddStyles();
            AddMiniMapStyles();
        }

        


        #region Overridded Methods
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            
            ports.ForEach(port =>
            {

                if(port == startPort)
                {
                    return;
                }

                if(startPort.node == port.node)
                {
                    return;
                }

                if(startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        #endregion

        

        #region Manipulators
        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            SelectionArea select = new SelectionArea(this, parentWindow);
            select.OnEnable();

            this.AddManipulator(new TexAnimRectangleSelector(parentWindow));
            /*RectangleSelector rectangleSelector = new RectangleSelector();
            this.AddManipulator(new RectangleSelector());*/

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Entry State)", TexAnimAnimationNodeType.EntryState));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Animation State)", TexAnimAnimationNodeType.AnimationState));

            this.AddManipulator(CreateGroupContextualMenu());
        }


        private IManipulator CreateNodeContextualMenu(string actionTitle, TexAnimAnimationNodeType animationtype)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(animationtype, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
                
            );

            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Create Group", actionEvent => AddElement(CreateGroup("Animations Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))

            );

            return contextualMenuManipulator;
        }
        #endregion


        #region Elements Creation
        private Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group();
            group.title = title;

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            _visuals.Add(group);
            return group;
        }

        private TexAnimNode CreateNode(TexAnimAnimationNodeType animationType, Vector2 position)
        {
            Type nodeType = Type.GetType($"TexAnim.Elements.TexAnim{animationType}Node");
            TexAnimNode node = (TexAnimNode)Activator.CreateInstance(nodeType);
            TexAnim_SavedNode savedNode = parentWindow.controller.CreateSavedNode(animationType, position);
            node.Initialize(this, savedNode, position);
            node.Draw();

            _visuals.Add(node);
            return node;
        }

        private TexAnimNode RecreateNodeFromSaved(TexAnim_SavedNode savedNode)
        {
            Type nodeType = Type.GetType($"TexAnim.Elements.TexAnim{savedNode.nodeType}Node");
            TexAnimNode node = (TexAnimNode)Activator.CreateInstance(nodeType);


            node.Initialize(this, savedNode, savedNode.position);
            node.Draw();

            _visuals.Add(node);
            return node;
        }


        #endregion

        #region Callbacks

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<TexAnimNode> nodesToDelete = new List<TexAnimNode>();

                foreach(GraphElement element in selection)
                {
                    if(element is TexAnimNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }
                }

                foreach(TexAnimNode node in nodesToDelete)
                {
                    RemoveElement(node);
                }
            };
        }

        #endregion

        #region Header Functions

        private void ToggleMiniMap()
        {
            _miniMap.visible = !_miniMap.visible;
        }

        #endregion
        #region Repeated Elements




        #endregion

        #region Element Addition
        private VisualElement AddGraphViewHeader()
        {
            VisualElement graphViewHeader = new VisualElement();
            graphViewHeader.style.alignItems = Align.Center;

            animatorNameLabel = new Label();
            animatorNameLabel.text = "File Name: None";

            Button miniMapButton = TexAnimElementUtility.CreateButton("MiniMap", () => ToggleMiniMap());

            graphViewHeader.Add(animatorNameLabel);
            graphViewHeader.Add(miniMapButton);


            graphViewHeader.styleSheets.Add(TexAnimAnimatorEditorWindow.FindAndCheckStyle("TexAnimToolbarStyles.uss"));
            graphViewHeader.styleSheets.Add(TexAnimAnimatorEditorWindow.FindAndCheckStyle("TexAnimToolbarStyles.uss"));

            this.Add(graphViewHeader);
            return graphViewHeader;
        }

        

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddMiniMap()
        {
            _miniMap = new MiniMap()
            {
                anchored = true,
            };

            _miniMap.SetPosition(new Rect(-350, 50, 200, 150));

            Add(_miniMap);
        }

        private void AddStyles()
        {
            styleSheets.Add(TexAnimAnimatorEditorWindow.FindAndCheckStyle("TexAnimGraphViewStyle.uss"));

            styleSheets.Add(TexAnimAnimatorEditorWindow.FindAndCheckStyle("TexAnimNodeStyles.uss"));

            //styleSheets.Add(TexAnimAnimatorEditorWindow.FindAndCheckStyle("TexAnimNodeSliderStyle.uss"));
        }

        private void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29,29,30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            _miniMap.style.backgroundColor = backgroundColor;
            _miniMap.style.borderTopColor = borderColor;
            _miniMap.style.borderBottomColor = borderColor;
            _miniMap.style.borderRightColor = borderColor;
            _miniMap.style.borderLeftColor = borderColor;

        }


        #endregion

        #region Utilities

        public Vector2 GetLocalMousePosition(Vector2 mousePosition)
        {
            Vector2 worldMousePosition = mousePosition;
            Vector2 splitViewOffset = new Vector2(parentWindow.parametersView.style.width.value.value, 0);
            Vector2 localMousePosition =  contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        public void ActualizeGraphView()
        {
            Debug.Log("Before Visuals Count: " + _visuals.Count);

            for (int i = 0; i < _visuals.Count; i++)
            {
                Debug.Log("Processing: " + i);

                RemoveElement(_visuals[i]);

                _visuals[i] = null;
                _visuals.RemoveAt(i);
            }

            Debug.Log("After Visuals Count: " + _visuals.Count);

            foreach (KeyValuePair<string, TexAnim_SavedNode> savedNode in parentWindow.controller.SavedNodes)
            {

                TexAnimNode newNode = RecreateNodeFromSaved(savedNode.Value);
                AddElement(newNode);
            }
        }

        #endregion
    }

}