using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using TexAnim.Data;
using UnityEditor;
using UnityEngine;


namespace TexAnim.Data
{
    using System;
    using TexAnim.Data.Settings;
    using TexAnim.Enumerations;
    using TexAnim.Editor.Utilities;
    using UnityEditor;

    [CreateAssetMenu(fileName = "New TexAnimatorController", menuName = "TexAnim/TexAnimator Controller", order = 1)]
    public class TexAnimatorController : ScriptableObject
    {
        // Animator Nodes
        private SerializableDictionary<string, TexAnim_SavedNode> _savedNodes;

        // Animator Parameters
        private SerializableDictionary<string, TexAnim_AnimsSettings> _animatorParameters;
        private List<string> _parameters = new List<string>();
        private bool _isInitialized;


        public SerializableDictionary<string, TexAnim_SavedNode> SavedNodes { get => _savedNodes; set => _savedNodes = value; }
        public SerializableDictionary<string, TexAnim_AnimsSettings> AnimatorParameters { get => _animatorParameters; set => _animatorParameters = value; }
        public List<string> Parameters { get => _parameters; set => _parameters = value; }
        public bool IsInitialized { get => _isInitialized;}


        public TexAnim_SavedNode CreateSavedNode(TexAnimAnimationNodeType nodeType, Vector2 position)
        {
            string baseName = TexAnimDataUtility.GetNodePrefix(nodeType);

            string nodeName = "";
            if (_savedNodes.ContainsKey(baseName))
            {
                nodeName = TexAnimDataUtility.GetNewNodeName(nodeType, _savedNodes);
            }
            else
            {
                nodeName = baseName;
            }

            TexAnim_SavedNode newSavedNode = new TexAnim_SavedNode(nodeName, nodeType, position);
            _savedNodes.Add(nodeName, newSavedNode);

            return newSavedNode;
        }

        public void AddAnimatorParameter(TexAnim_SettingsType type)
        {
            string baseName = TexAnimDataUtility.GetParameterPrefix(type);

            if (_animatorParameters.ContainsKey(baseName))
            {
                string parameterName = TexAnimDataUtility.GetNewParameterName(type, _animatorParameters);

                _animatorParameters.Add(parameterName, new TexAnim_AnimsSettings(type));
                _parameters.Add(parameterName);
            }
            else
            {
                string parameterName = baseName;

                _animatorParameters.Add(parameterName, new TexAnim_AnimsSettings(type));
                _parameters.Add(parameterName);
            }
            
        }

        public void RenameAnimatorNode(string currentName, string newName)
        {
            // Duplicating the parameter in a fully new instance.
            TexAnim_SavedNode savedNode = new TexAnim_SavedNode();
            TexAnim_SavedNode originalInstance = _savedNodes[currentName];

            newName = TexAnimDataUtility.GetNodeIndexedName(newName, _savedNodes);

            savedNode.nodeName = newName;
            savedNode.nodeType = originalInstance.nodeType;
            savedNode.position = originalInstance.position;
            savedNode.speed = originalInstance.speed;
            savedNode.texAnimClip = originalInstance.texAnimClip;



            // Deleting the old key pair.
            _savedNodes.Remove(currentName);

            _savedNodes.Add(newName, savedNode);
        }



        public void RenameAnimatorParameter(int parameterIndex, string newName)
        {
            // Duplicating the parameter in a fully new instance.
            TexAnim_AnimsSettings savedSettings = new TexAnim_AnimsSettings(TexAnim_SettingsType.New);
            TexAnim_AnimsSettings originalInstance = _animatorParameters[_parameters[parameterIndex]];

            savedSettings.settingsType = originalInstance.settingsType;
            
            switch(savedSettings.settingsType)
            {
                case TexAnim_SettingsType.Float:
                    savedSettings.SetFloatValue(originalInstance.GetFloatValue());
                    break;

                case TexAnim_SettingsType.Int:
                    savedSettings.SetIntValue(originalInstance.GetIntValue());
                    break;

                case TexAnim_SettingsType.Bool:
                    savedSettings.SetBoolValue(originalInstance.GetBoolValue());
                    break;

                case TexAnim_SettingsType.Trigger:
                    savedSettings.SetTriggerValue(originalInstance.GetTriggerValue());
                    break;

            }


            // Deleting the old key pair.
            _animatorParameters.Remove(_parameters[parameterIndex]);

            _animatorParameters.Add(newName, savedSettings);
            _parameters[parameterIndex] = newName;
        }

        public TexAnim_AnimsSettings GetTexAnimSettings(int index)
        {
            return _animatorParameters[_parameters[index]];
        }



        public void Initialized()
        {
            TexAnim_SavedNode entryNode = TexAnimDataUtility.GetEntryNode();
            _savedNodes.Add(entryNode.nodeName, entryNode);

            _isInitialized = true;
        }
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(TexAnimatorController))]
    public class TexAnimatorControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            /*// Reference to the target script
            TexAnimatorController animator = (TexAnimatorController)target;

            DrawDefaultInspector();


            if (!animator.IsInitialized)
            {
                if (GUILayout.Button("Initialised"))
                {
                    animator.Initialized();
                }
            }*/
            
        }
    }

#endif
}