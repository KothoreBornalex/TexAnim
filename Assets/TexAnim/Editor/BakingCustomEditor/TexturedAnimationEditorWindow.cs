using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using static BakingAnimationsMachine;

public class TexturedAnimationEditorWindow : EditorWindow
{
    

    private static BakingAnimationsMachine _bakingAnimationsMachine;

    private string _basePath = "";
    private string _folderName = "";
    private string _savePath = "";

    private BakingType _bakingType;
    private GameObject AnimatedObject;
    private AnimationClip SingleAnimation;
    private string _animationName;

    private UnBakedTextureAnimations unBakedTextureAnimations;





    [MenuItem("Tools/Textured Animation Creator")]
    public static void CreateEditorWindow()
    {
        EditorWindow window = GetWindow<TexturedAnimationEditorWindow>();
        window.titleContent = new GUIContent("Textured Animation Editor");

        _bakingAnimationsMachine = new BakingAnimationsMachine();
    }


    private void OnGUI()
    {
        HandleRegenerateWindowButton();
        HandleSavePathButton();

        HandleBakingTypeChoice();


        switch (_bakingType)
        {
            case BakingType.SingleAnimation: 
                HandleSingleAnimation(); 
                break;

            case BakingType.BlendingAnimation:

                break;

            case BakingType.MultipleAnimations:

                break;
        }

        EditorGUILayout.Space(20);




        EditorGUILayout.Space(20);
        GUI.enabled = SingleAnimation != null && AnimatedObject != null;
        if (GUILayout.Button("Generate Textured Animation"))
        {
            switch (_bakingType)
            {
                case BakingType.SingleAnimation:
                    _bakingAnimationsMachine.LaunchBaking(_bakingType, SingleAnimation, AnimatedObject, _animationName, _savePath);
                    break;

                case BakingType.BlendingAnimation:
                    break;

                case BakingType.MultipleAnimations:
                    break;
            }


            Debug.Log("Debug Test: " + _basePath + _animationName);
        }

    }

    private void HandleRegenerateWindowButton()
    {
        EditorGUILayout.Space(20);
        if (GUILayout.Button("Regenerate Window"))
        {
            _bakingAnimationsMachine = new BakingAnimationsMachine();
        }
        EditorGUILayout.Space(20);
    }

    private string OpenExplorer(string basePath)
    {
        string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", basePath, "");
        string result = "";

        // Find the index of "Assets"
        int assetsIndex = selectedPath.IndexOf("Assets");

        // Check if "Assets" is found in the string
        if (assetsIndex != -1)
        {
            // Extract the substring starting from "Assets"
            result = selectedPath.Substring(assetsIndex);
        }
        else
        {
            // Handle the case where "Assets" is not found
            Debug.Log("Error: 'Assets' not found in the input string.");
        }

        return result;
    }







    private void HandleSavePathButton()
    {
        EditorGUILayout.Space(10);
        if (GUILayout.Button("Select Save Path"))
        {
            Debug.Log("Is Working");
            _savePath = OpenExplorer(_basePath);
        }

        _savePath = EditorGUILayout.TextField("Save Path:", _savePath);

        EditorGUILayout.Space(20);
    }

    private void HandleBakingTypeChoice()
    {
        BakingType newBakingType = (BakingType)EditorGUILayout.EnumPopup("Baking Type", _bakingType);
        _bakingType = newBakingType;
    }

    private void HandleSingleAnimation()
    {
        GameObject newAnimatedObject = EditorGUILayout.ObjectField("Mesh Rig", AnimatedObject, typeof(GameObject), true) as GameObject;
        AnimationClip newAnimationClip = EditorGUILayout.ObjectField("Animation To Convert", SingleAnimation, typeof(AnimationClip), true) as AnimationClip;


        if (newAnimationClip != SingleAnimation && newAnimationClip != null)
        {
            //_animationName = newAnimatedObject.name + "_" + newAnimationClip.name + "_TexturedAnimations";
            _animationName = newAnimatedObject.name + "_" + newAnimationClip.name + "_TexAnim";
        }

        AnimatedObject = newAnimatedObject;
        SingleAnimation = newAnimationClip;
        _animationName = EditorGUILayout.TextField("Animation Name:", _animationName);
    }

}
