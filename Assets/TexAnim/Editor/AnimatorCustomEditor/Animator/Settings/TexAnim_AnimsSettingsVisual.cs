using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TexAnim.Data.Settings;
using TexAnim.Editor.Utilities;
using TexAnim.Enumerations;
using TexAnim.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TexAnim.Data
{
    public class TexAnim_AnimsSettingsVisual : VisualElement
    {
        private int _parameterIndex;
        private string _parameterKey;
        private TexAnim_AnimsSettings _parameterValue;
        private TexAnimAnimatorEditorWindow _editorWindow;


        private TextField _textField;

        private FloatField floatField;
        private IntegerField integerField;
        private Toggle boolField;
        private RadioButton triggerField;


        public TexAnim_AnimsSettingsVisual(TexAnimAnimatorEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;

            this.style.flexDirection = FlexDirection.Row;

            _textField = new TextField();
            _textField.style.minWidth = 125;
            _textField.RegisterValueChangedCallback(OnTextFieldValueChanged);
            this.Add(_textField);

            floatField = new FloatField();
            integerField = new IntegerField();
            boolField = new Toggle();
            triggerField = new RadioButton();

            floatField.RegisterValueChangedCallback(OnFloatFieldValueChanged);
            integerField.RegisterValueChangedCallback(OnIntFieldValueChanged);
            boolField.RegisterValueChangedCallback(OnBoolFieldValueChanged);
            triggerField.RegisterValueChangedCallback(OnTriggerFieldValueChanged);

            SetParameterFieldStyle(floatField);
            SetParameterFieldStyle(integerField);
            SetParameterFieldStyle(boolField);
            SetParameterFieldStyle(triggerField);

        }

        

        private void SetParameterFieldStyle(VisualElement field)
        {
            field.style.paddingLeft = 30;
            field.style.paddingRight = 10;

            field.style.minWidth = 80;

        }
        public void Initialize(int i)
        {
            Debug.Log("Index: " + i);
            //Set the current linked anim setting.
            _parameterIndex = i;
            _parameterKey = _editorWindow.controller.Parameters[_parameterIndex];
            _parameterValue = _editorWindow.controller.AnimatorParameters[_parameterKey];


            // Clear current visual Element.
            if (this.Contains(floatField)) this.Remove(floatField);
            if(this.Contains(integerField)) this.Remove(integerField);
            if(this.Contains(boolField)) this.Remove(boolField);
            if(this.Contains(triggerField)) this.Remove(triggerField);


            //Initialize the Visual Element
            _textField.SetValueWithoutNotify(_editorWindow.controller.Parameters[_parameterIndex]);

            switch (_parameterValue.settingsType)
            {
                case TexAnim_SettingsType.Float:
                    floatField.SetValueWithoutNotify(_parameterValue.GetFloatValue());
                    this.Add(floatField); break;
                case TexAnim_SettingsType.Int:
                    integerField.SetValueWithoutNotify(_parameterValue.GetIntValue());
                    this.Add(integerField); break;
                case TexAnim_SettingsType.Bool:
                    boolField.SetValueWithoutNotify(_parameterValue.GetBoolValue());
                    this.Add(boolField); break;
                case TexAnim_SettingsType.Trigger:
                    triggerField.SetValueWithoutNotify(_parameterValue.GetTriggerValue());
                    this.Add(triggerField); break;

                default:
                    throw new ArgumentException("Invalid element type");
            }
        }



        void OnTextFieldValueChanged(ChangeEvent<string> evt)
        {
            string newName = TexAnimDataUtility.GetParameterIndexedName(evt.newValue, _editorWindow.controller.AnimatorParameters);
            _textField.SetValueWithoutNotify(newName);
            _editorWindow.controller.RenameAnimatorParameter(_parameterIndex, newName);

            _parameterValue = _editorWindow.controller.GetTexAnimSettings(_parameterIndex);
        }

        void OnFloatFieldValueChanged(ChangeEvent<float> evt)
        {
            _parameterValue.SetFloatValue(evt.newValue);
        }
        void OnIntFieldValueChanged(ChangeEvent<int> evt)
        {
            _parameterValue.SetIntValue(evt.newValue);
        }

        void OnBoolFieldValueChanged(ChangeEvent<bool> evt)
        {
            _parameterValue.SetBoolValue(evt.newValue);
        }

        void OnTriggerFieldValueChanged(ChangeEvent<bool> evt)
        {
            _parameterValue.SetTriggerValue(evt.newValue);
        }

    }
}
