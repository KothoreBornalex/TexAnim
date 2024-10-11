using UnityEditor;
using UnityEngine;

public class FocusProjectWindow : EditorWindow
{
    [MenuItem("Window/Focus Project Window Example")]
    public static void ShowWindow()
    {
        GetWindow<FocusProjectWindow>("Focus Project Window");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Focus on File"))
        {
            // Provide the path to the file you want to focus on
            string filePath = "Assets/Imports/2D/TexturedAnimations/Grunt/GruntPolyart CLEAR_Run_TexAnim.asset"; // Replace with your actual file path

            FocusOnFile(filePath);
        }
    }

    private static void FocusOnFile(string filePath)
    {
        // Focus on the specified file in the Project window
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(filePath);
    }
}