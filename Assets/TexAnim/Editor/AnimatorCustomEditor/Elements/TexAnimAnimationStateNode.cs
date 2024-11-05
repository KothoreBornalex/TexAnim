using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScriptableObject_AnimationsBank;

namespace TexAnim.Elements
{
    using Enumerations;
    using TexAnim.Data;
    using TexAnim.Editor.Utilities;
    using TexAnim.Windows;
    using UnityEditor;
    using UnityEditor.Experimental.GraphView;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    public class TexAnimAnimationStateNode : TexAnimNode
    {
        //public Slider slider;
        public override void Initialize(TexAnimGraphView graphView,TexAnim_SavedNode savedNode, Vector2 position)
        {
            base.Initialize(graphView, savedNode, position);
            NodeType = TexAnimAnimationNodeType.AnimationState;

            Choices.Add("Next Animation");
        }

        public override void Draw()
        {
            base.Draw();

            // INPUT CONTAINER
            Port inputPort = TexAnimElementUtility.CreatePort(this, "State Entry", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            // EXTENSIONS CONTAINER
            //Slider slider = new Slider();
            ProgressBar slider = new ProgressBar();
            extensionContainer.Add(slider);

            /*TexturedAnimation textured = new TexturedAnimation();
            extensionContainer.Add(textured);*/

            /*//BuildCustomContainer();
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("tex-node_custom-data-container");
            extensionContainer.Add(customDataContainer);*/

            //Handles.DrawLine(new Vector3(0, 0, 0), new Vector3(100, 0, 0));
            // OUTPUT CONTAINER
            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        public void BuildCustomContainer()
        {
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("tex-node_custom-data-container");
            Foldout animationDescriptionFoldout = TexAnimElementUtility.CreateFoldout("Node Description", false);


            TextField animationDescriptionTextField = TexAnimElementUtility.CreateTextArea(NodeName);
            animationDescriptionTextField.AddToClassList("tex-node_textfield");
            animationDescriptionTextField.AddToClassList("tex-node_quote-textfield");

            animationDescriptionFoldout.Add(animationDescriptionTextField);

            customDataContainer.Add(animationDescriptionFoldout);
            extensionContainer.Add(customDataContainer);
        }
    }
}