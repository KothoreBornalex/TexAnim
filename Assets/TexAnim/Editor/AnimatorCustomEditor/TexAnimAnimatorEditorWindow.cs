using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TexAnim.Windows
{
    using System.Collections;
    using TexAnim.Data;
    using TexAnim.Editor.Utilities;
    using TexAnim.Enumerations;
    using UnityEditor.IMGUI.Controls;
    using static UnityEngine.Rendering.DebugUI.Table;

    public class TexAnimAnimatorEditorWindow : EditorWindow
    {
        private string defaultFileName = "AnimatorControllerName";

        public TexAnimatorController controller;
        public SplitView splitView;
        public VisualElement leftSideSplitView;
        public ParametersView parametersView;
        public InspectorView inspectorView;
        public TexAnimGraphView graphView;
        public VisualElement graphViewHeader;
        public Toolbar footer;
        public Label nullControllerMessage;
        public Button initializedControllerButton;

        [MenuItem("Window/TexAnim/AnimatorEditorWindow")]
        public static void Open()
        {
            GetWindow<TexAnimAnimatorEditorWindow>("TexAnim Animator Controller");
        }


        private void OnEnable()
        {
            ActualizeAniator();
        }






        private void OnSelectionChange()
        {
            ActualizeAniator();
        }



        private void ActualizeAnimatorEditorWindow()
        {
            graphView.animatorNameLabel.text = "File Name: " + controller.name;
        }







        
        #region Find File Function
        public static StyleSheet FindAndCheckStyle(string fileName)
        {
            string filePath = FindFile(fileName);

            if (!string.IsNullOrEmpty(filePath))
            {
                //Debug.Log("Path to " + fileName + ": " + filePath);
            }
            else
            {
                Debug.LogWarning(fileName + " not found!");
            }

            string relativePath = filePath.Replace(Application.dataPath, "Assets");
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load(relativePath);

            return styleSheet;
        }

        public static string FindFile(string fileName)
        {
            string[] filePaths = Directory.GetFiles(Application.dataPath, fileName, SearchOption.AllDirectories);

            if (filePaths.Length > 0)
            {
                return filePaths[0]; // Return the first matching file path
            }
            else
            {
                return null; // Return null if the file is not found
            }
        }
        #endregion

        #region Elements Additions

        private void AddBasedAnimatorWindowElements()
        {
            if(splitView == null) splitView = AddSplitView();
            //if (parametersView == null) parametersView = AddParametersView();
            if(leftSideSplitView == null) leftSideSplitView = AddLeftSideSplitView();
            if (graphView == null) graphView = AddGraphView();

            AddStyles();

            ActualizeAnimatorEditorWindow();
            graphView.ActualizeGraphView();
            parametersView.ActualizeSettingsPanelView();
        }


        private void RemoveBasedAnimatorWindowElements()
        {
            /*if (parametersView != null)
            {
                splitView.Remove(parametersView);
                parametersView = null;
            }*/

            if (leftSideSplitView != null)
            {
                leftSideSplitView.Remove(parametersView);
                leftSideSplitView.Remove(inspectorView);
                splitView.Remove(leftSideSplitView);

                leftSideSplitView = null;
                parametersView = null;
                inspectorView = null;
            }

            if (graphView != null)
            {
                splitView.Remove(graphView);
                graphView = null;
            }

            if (splitView != null)
            {
                rootVisualElement.Remove(splitView);
                splitView = null;
            }

            

          
        }

        private void ToggleBasedAnimatorWindowElements(bool state)
        {
            if (!state)
            {
                RemoveBasedAnimatorWindowElements();
            }
            else
            {
                AddBasedAnimatorWindowElements();
            }

        }



        private void AddNullControllerMessage()
        {
            if(nullControllerMessage == null)
            {
                nullControllerMessage = new Label();
                nullControllerMessage.text = "No Tex Animator Controller Selected Yet ! Or It Is Not Initialised Yet !";
                nullControllerMessage.style.fontSize = 30;


                rootVisualElement.Add(nullControllerMessage);
            }
            
        }

        private void RemoveNullControllerMessage()
        {
            if (nullControllerMessage == null) return;
            else
            {
                rootVisualElement.Remove(nullControllerMessage);
                nullControllerMessage = null;
            }
            
        }

        private void ToggleNullControllerMessage(bool state)
        {
            if (!state)
            {
                RemoveNullControllerMessage();
            }
            else
            {
                AddNullControllerMessage();
            }

        }

        private void AddInitializedControllerButton()
        {
            if (initializedControllerButton == null)
            {
                initializedControllerButton = TexAnimElementUtility.CreateButton("Initialized Controller", () => LinkToControllerInitialization());

                initializedControllerButton.style.paddingBottom = this.minSize.y / 2;
                initializedControllerButton.style.paddingTop = this.minSize.y /2;


                initializedControllerButton.style.marginTop = this.minSize.y;
                initializedControllerButton.style.marginBottom = this.minSize.y;

                initializedControllerButton.style.marginLeft = this.minSize.x;
                initializedControllerButton.style.marginRight = this.minSize.x;

                rootVisualElement.Add(initializedControllerButton);
            }

        }

        private void RemoveInitializedControllerButton()
        {
            if (initializedControllerButton == null) return;
            else
            {
                rootVisualElement.Remove(initializedControllerButton);
                initializedControllerButton = null;
            }

        }

        private void ToggleInitializedControllerButton(bool state)
        {
            if (!state)
            {
                RemoveInitializedControllerButton();
            }
            else
            {
                AddInitializedControllerButton();
            }
            
        }


        private SplitView AddSplitView()
        {
            SplitView splitView = new SplitView();

            splitView.fixedPaneInitialDimension = 250;
            rootVisualElement.Add(splitView);

            return splitView;
        }

        TexAnimGraphView AddGraphView()
        {
            TexAnimGraphView graphView = new TexAnimGraphView(this);
            splitView.Add(graphView);

            return graphView;
        }



        private VisualElement AddLeftSideSplitView()
        {
            VisualElement leftSplitViewSide = new VisualElement();
            leftSplitViewSide.style.width = 200;
            leftSplitViewSide.style.flexDirection = FlexDirection.Column;


            //leftSplitViewSide.style.alignItems = Align.Center;


            parametersView = AddParametersView();
            leftSplitViewSide.Add(parametersView);

            inspectorView = AddInspectorView();
            leftSplitViewSide.Add(inspectorView);

            splitView.Add(leftSplitViewSide);
            return leftSplitViewSide;
        }

        private ParametersView AddParametersView()
        {
            /*InspectorView inspector = new InspectorView(this);
            inspector.style.width = 200;
            splitView.Add(inspector);*/

            ParametersView parametersView = new ParametersView(this);
            //parametersView.style.width = 200;
            parametersView.style.flexGrow = 1;
            return parametersView;
        }


        private InspectorView AddInspectorView()
        {

            InspectorView inspectorView = new InspectorView(this);
            //inspectorView.style.width = 200;
            inspectorView.style.flexGrow = 1;

            return inspectorView;
        }


        private void AddStyles()
        {
            rootVisualElement.styleSheets.Add(FindAndCheckStyle("TexAnimVariables.uss"));
        }


        #endregion

        #region Animator Window Interactions
        private void SetBasedAnimatorWindowElements(bool state)
        {
            if (state)
            {
                splitView.visible = true;
                parametersView.visible = true;
                graphView.visible = true;

                ActualizeAnimatorEditorWindow();
                graphView.ActualizeGraphView();
                parametersView.ActualizeSettingsPanelView();
            }
            else
            {
                splitView.visible = false;
                parametersView.visible = false;
                graphView.visible = false;
            }
        }




        private void LinkToControllerInitialization()
        {
            controller.Initialized();
            ActualizeAniator();
        }




        /*private void ActualizeAniator()
        {
            TexAnimatorController animatorController = Selection.activeObject as TexAnimatorController;

            if(animatorController) controller = animatorController;

            if (controller == null)
            {
                SetNullControllerMessageState(true);
                SetBasedAnimatorWindowElements(false);
                SetInitializedControllerButtonState(false);
            }
            else if (controller.IsInitialized)
            {
                SetBasedAnimatorWindowElements(true);
                SetNullControllerMessageState(false);
                SetInitializedControllerButtonState(false);
            }
            else if (!controller.IsInitialized)
            {
                SetInitializedControllerButtonState(true);
                SetNullControllerMessageState(false);
                SetBasedAnimatorWindowElements(false);
            }
        }*/


        private void ActualizeAniator()
        {
            TexAnimatorController animatorController = Selection.activeObject as TexAnimatorController;

            if (animatorController) controller = animatorController;

            if (controller == null)
            {
                ToggleNullControllerMessage(true);
                ToggleBasedAnimatorWindowElements(false);
                ToggleInitializedControllerButton(false);
            }
            else if (controller.IsInitialized)
            {
                ToggleBasedAnimatorWindowElements(true);
                ToggleNullControllerMessage(false);
                ToggleInitializedControllerButton(false);
            }
            else if (!controller.IsInitialized)
            {
                ToggleNullControllerMessage(false);
                ToggleBasedAnimatorWindowElements(false);
                ToggleInitializedControllerButton(true);
            }
        }
        #endregion

    }
}