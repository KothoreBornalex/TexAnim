using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace TexAnim.Windows
{
    using System;
    using TexAnim.Enumerations;
    using TexAnim.Data;
    using UnityEditor.UIElements;

    public class ParametersView : VisualElement
    {

        private TexAnimAnimatorEditorWindow _editorWindow;

        public VisualElement settingsPanelHeader;
        private PopupField<TexAnim_SettingsType> _popupField;
        public Label popUpName;

        public ListView listView;

        private TexAnim_SettingsType selectedOption;


        public ParametersView(TexAnimAnimatorEditorWindow texAnimAnimatorEditorWindow)
        {
            _editorWindow = texAnimAnimatorEditorWindow;
            settingsPanelHeader = AddSettingsPanelHeader();

            AddSettingsButton();

            AddParametersList();
        }

        private void AddParametersList()
        {
            Func<TexAnim_AnimsSettingsVisual> makeItem = () => new TexAnim_AnimsSettingsVisual(_editorWindow);

            Action<VisualElement, int> bindItem = (e, i) => (e as TexAnim_AnimsSettingsVisual).Initialize(i);

            listView = new ListView();
            listView.reorderable = true;
            listView.reorderMode = ListViewReorderMode.Animated;
            listView.showBorder = true;
            listView.showBoundCollectionSize = true;
            listView.makeItem = makeItem;
            listView.bindItem = bindItem;
            listView.itemsSource = _editorWindow.controller.Parameters;
            listView.selectionType = SelectionType.Multiple;

            this.Add(listView);
        }

        private VisualElement CreateParamaterElement()
        {
            VisualElement paramater = new VisualElement();

            return paramater;
        }



        private VisualElement AddSettingsPanelHeader()
        {
            VisualElement settingsPanelHeader = new VisualElement();

            settingsPanelHeader.styleSheets.Add(TexAnimAnimatorEditorWindow.FindAndCheckStyle("TexAnimToolbarStyles.uss"));
            this.Add(settingsPanelHeader);

            return settingsPanelHeader;
        }


        public void AddSettingsButton()
        {
            List< TexAnim_SettingsType> choices = new List<TexAnim_SettingsType>(); // Assuming TexAnimTransitionSettingsType is your enum type
            choices.Add(TexAnim_SettingsType.New);
            choices.Add(TexAnim_SettingsType.Float);
            choices.Add(TexAnim_SettingsType.Int);
            choices.Add(TexAnim_SettingsType.Bool);
            choices.Add(TexAnim_SettingsType.Trigger);

            _popupField = new PopupField<TexAnim_SettingsType>("Anim Settings:", choices, TexAnim_SettingsType.New);
            
            _popupField.RegisterValueChangedCallback(OnOptionSelected);
            settingsPanelHeader.Add(_popupField);
        }

        private void OnOptionSelected(ChangeEvent<TexAnim_SettingsType> evt)
        {
            switch (evt.newValue)
            {
                case TexAnim_SettingsType.New:
                    // Nothing
                    break;

                case TexAnim_SettingsType.Float:
                    Debug.Log("Float");
                    _editorWindow.controller.AddAnimatorParameter(evt.newValue);
                    break;

                case TexAnim_SettingsType.Int:
                    _editorWindow.controller.AddAnimatorParameter(evt.newValue);
                    Debug.Log("Int");
                    break;

                case TexAnim_SettingsType.Bool:
                    _editorWindow.controller.AddAnimatorParameter(evt.newValue);
                    Debug.Log("Bool");
                    break;

                case TexAnim_SettingsType.Trigger:
                    _editorWindow.controller.AddAnimatorParameter(evt.newValue);
                    Debug.Log("Trigger");
                    break;

            }

            EndSelection();
            listView.Rebuild();

        }

        private void EndSelection()
        {
            _popupField.value = TexAnim_SettingsType.New;
        }


        public void ActualizeSettingsPanelView()
        {
            Remove(listView);
            listView = null;
            AddParametersList();
        }
    }
}