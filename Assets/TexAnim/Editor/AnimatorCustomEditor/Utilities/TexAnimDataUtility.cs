using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TexAnim.Editor.Utilities
{
    using TexAnim.Data;
    using TexAnim.Data.Settings;
    using TexAnim.Enumerations;

    public static class TexAnimDataUtility
    {

        public static TexAnim_SavedNode GetEntryNode()
        {
            TexAnim_SavedNode entryNode = new TexAnim_SavedNode();
            entryNode.nodeName = "Entry Node";
            entryNode.nodeType = TexAnimAnimationNodeType.EntryState;
            entryNode.position = new Vector2(0, 0);

            return entryNode;
        }


        public static string GetNodePrefix(TexAnimAnimationNodeType type)
        {
            string prefix = "";

            switch (type)
            {
                case TexAnimAnimationNodeType.AnimationState:
                    prefix = "Animation State";
                    break;

                case TexAnimAnimationNodeType.EntryState:
                    prefix = "Entry State";
                    break;

            }

            return prefix;
        }

        public static string GetNewNodeName(TexAnimAnimationNodeType type, SerializableDictionary<string, TexAnim_SavedNode> savedNode)
        {
            string baseName = GetNodePrefix(type);


            string nodeName = "";
            for (int i = 0; i < 999; i++)
            {
                nodeName = baseName + " " + i;
                if (!savedNode.ContainsKey(nodeName))
                {
                    return nodeName;
                }
            }

            return "Error";
        }

        public static string GetNodeIndexedName(string currentName, SerializableDictionary<string, TexAnim_SavedNode> savedNode)
        {
            string baseName = currentName;

            if(!savedNode.ContainsKey(baseName)) return baseName;

            string nodeName = "";
            for (int i = 0; i < 999; i++)
            {
                nodeName = baseName + " " + i;
                if (!savedNode.ContainsKey(nodeName))
                {
                    return nodeName;
                }
            }

            return "Error";
        }

        public static string GetParameterPrefix(TexAnim_SettingsType type)
        {
            string prefix = "";

            switch (type)
            {
                case TexAnim_SettingsType.Float:
                    prefix = "New Float";
                    break;

                case TexAnim_SettingsType.Int:
                    prefix = "New Int";
                    break;

                case TexAnim_SettingsType.Bool:
                    prefix = "New Bool";
                    break;

                case TexAnim_SettingsType.Trigger:
                    prefix = "New Trigger";
                    break;
            }

            return prefix;
        }

        public static string GetNewParameterName(TexAnim_SettingsType type, SerializableDictionary<string, TexAnim_AnimsSettings> _animatorParameters)
        {
            string baseName = GetParameterPrefix(type);


            string parameterName = "";
            for (int i = 0; i < 999; i++)
            {
                parameterName = baseName + " " + i;
                if (!_animatorParameters.ContainsKey(parameterName))
                {
                    return parameterName;
                }
            }

            return "Error";
        }


        public static string GetParameterIndexedName(string newName, SerializableDictionary<string, TexAnim_AnimsSettings> _animatorParameters)
        {
            string baseName = newName;

            if (!_animatorParameters.ContainsKey(baseName)) return baseName;


            string parameterName = "";
            for (int i = 0; i < 999; i++)
            {
                parameterName = baseName + " " + i;
                if (!_animatorParameters.ContainsKey(parameterName))
                {
                    return parameterName;
                }
            }

            return "Error";
        }
    }
}
