using System;
using System.Collections;
using System.Collections.Generic;
using TexAnim.Elements;
using TexAnim.Enumerations;
using TexAnim.Windows;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace TexAnim
{
    public class InspectorView : VisualElement
    {

        private TexAnimAnimatorEditorWindow _editorWindow;

        public VisualElement inspectorPanelHeader;
        public VisualElement contentPanel;
        private TexAnimNode _currentNode;


        private FloatField _motionLengthField;
        private FloatField _speedField;


        public InspectorView(TexAnimAnimatorEditorWindow texAnimAnimatorEditorWindow)
        {
            _editorWindow = texAnimAnimatorEditorWindow;

            inspectorPanelHeader = AddInspectorPanelHeader();
            contentPanel = AddContentPanel();
        }


        #region Adding Elements
        private VisualElement AddInspectorPanelHeader()
        {
            VisualElement inspectorPanelHeader = new VisualElement();
            Label panelTitle = new Label();


            inspectorPanelHeader.styleSheets.Add(TexAnimAnimatorEditorWindow.FindAndCheckStyle("TexAnimToolbarStyles.uss"));
            panelTitle.text = "Inspector View";


            inspectorPanelHeader.Add(panelTitle);
            this.Add(inspectorPanelHeader);

            return inspectorPanelHeader;
        }

        private VisualElement AddContentPanel()
        {
            VisualElement inspectorPanelHeader = new VisualElement();

            this.Add(inspectorPanelHeader);

            return inspectorPanelHeader;
        }

        internal void ActualizeInspector(object selectedObject)
        {
            TexAnimNode node = selectedObject as TexAnimNode;
            contentPanel.Clear();
            

            if (node != null)
            {
                DrawInspectorForNode(node);
            }
        }


        private void DrawInspectorForNode(TexAnimNode node)
        {
            _currentNode = node;

            Label objectName = new Label();
            objectName.text = "Object Name: " + node.NodeName;
            contentPanel.Add(objectName);
            objectName.style.marginBottom = 7;



            var texAnimClipField = new ObjectField();
            texAnimClipField.objectType = typeof(TexAnimClip);
            texAnimClipField.label = "Motion: ";
            if (_editorWindow.controller.SavedNodes[node.NodeName].texAnimClip != null) 
                texAnimClipField.value = _editorWindow.controller.SavedNodes[node.NodeName].texAnimClip;
            contentPanel.Add(texAnimClipField);


            _motionLengthField = new FloatField();
            _motionLengthField.label = "Motion Duration: ";
            _motionLengthField.isReadOnly = true;
            if (_editorWindow.controller.SavedNodes[node.NodeName].texAnimClip != null)
                _motionLengthField.value = _editorWindow.controller.SavedNodes[node.NodeName].texAnimClip.MotionLength;
            contentPanel.Add(_motionLengthField);
            

            FloatField speedField = new FloatField();
            speedField.label = "Speed";
            speedField.value = _editorWindow.controller.SavedNodes[node.NodeName].speed;
            contentPanel.Add(speedField);




            texAnimClipField.RegisterValueChangedCallback(OnTexAnimClipFieldChanged);
            speedField.RegisterValueChangedCallback(OnSpeedFieldChanged);

        }
        #endregion





        private void OnTexAnimClipFieldChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            _editorWindow.controller.SavedNodes[_currentNode.NodeName].texAnimClip = (TexAnimClip) evt.newValue;
            if (evt.newValue != null) _motionLengthField.value = _editorWindow.controller.SavedNodes[_currentNode.NodeName].texAnimClip.MotionLength;
            else
            {
                _motionLengthField.value = 0;
            }
        }

        private void OnSpeedFieldChanged(ChangeEvent<float> evt)
        {
            _editorWindow.controller.SavedNodes[_currentNode.NodeName].speed = evt.newValue;
        }
    }
}
