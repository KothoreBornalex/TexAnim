using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace TexAnim.Elements
{
    using Enumerations;
    using TexAnim.Data;
    using TexAnim.Editor.Utilities;
    using TexAnim.Windows;

    public class TexAnimNode : Node
    {
        private TexAnimGraphView _graphView;
        private TextField _nodeNameTextField;
        public string NodeName { get; set; }
        public List<string> Choices { get; set; }
        public string NodeDescription { get; set; }
        public TexAnimAnimationNodeType NodeType { get; set; }

        private StyleColor defaultBackgroundColor;

        private TexAnim_SavedNode _savedNode;

        public override void OnSelected()
        {
            base.OnSelected();
            _graphView.parentWindow.inspectorView.ActualizeInspector(this);
        }


        public virtual void Initialize(TexAnimGraphView graphView, TexAnim_SavedNode savedNode, Vector2 position)
        {
            _graphView = graphView;
            _savedNode = savedNode;
            NodeName = savedNode.nodeName;
            Choices = new List<string>();
            NodeDescription = "This is " + NodeName + " description";

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("tex-node_main-container");
            extensionContainer.AddToClassList("tex-node_extension-container");

            defaultBackgroundColor = mainContainer.style.backgroundColor;
        }

        public virtual void Draw()
        {
            // TITLE CONTAINER
            _nodeNameTextField = TexAnimElementUtility.CreateTextField(NodeName, null, callback =>
            {

                NodeName = callback.newValue;

            });

            _nodeNameTextField.AddToClassList("tex-node_textfield");
            _nodeNameTextField.AddToClassList("tex-node_filename-textfield");
            _nodeNameTextField.AddToClassList("tex-node_textfield_hidden");

            _nodeNameTextField.RegisterValueChangedCallback(OnNameTextFieldValueChanged);
            


            titleContainer.Insert(0, _nodeNameTextField);
        }

        void OnNameTextFieldValueChanged(ChangeEvent<string> evt)
        {
            string newName = TexAnimDataUtility.GetNodeIndexedName(evt.newValue, _graphView.parentWindow.controller.SavedNodes);
            _nodeNameTextField.SetValueWithoutNotify(newName);
            _graphView.parentWindow.controller.RenameAnimatorNode(evt.previousValue, newName);
            NodeName = newName;
            //Actualize Inspector based on new name.
            _graphView.parentWindow.inspectorView.ActualizeInspector(this);
        }




        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _savedNode.position = newPos.position;
        }



        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
    }
}
