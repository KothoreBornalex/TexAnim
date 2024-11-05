using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TexAnim.Elements
{
    using TexAnim.Editor.Utilities;
    using TexAnim.Enumerations;
    using TexAnim.Windows;
    using TexAnim.Data;

    public class TexAnimEntryStateNode : TexAnimNode
    {
        public override void Initialize(TexAnimGraphView graphView, TexAnim_SavedNode savedNode, Vector2 position)
        {
            base.Initialize(graphView, savedNode, position);
            NodeType = TexAnimAnimationNodeType.EntryState;

            Choices.Add("Start Animation");
            NodeDescription = "Entry Node is the starting node of your graph";

            mainContainer.AddToClassList("tex-entrynode_main-container");
            extensionContainer.AddToClassList("tex-entrynode_extension-container");
        }

        public override void Draw()
        {
            base.Draw();

            // EXTENSIONS CONTAINER

            // OUTPUT CONTAINER
            foreach (string choice in Choices)
            {
                Port choicePort = TexAnimElementUtility.CreatePort(this, choice, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);

                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

    }

}