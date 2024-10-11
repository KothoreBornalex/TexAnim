using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CustomSceneWindow : EditorWindow
{
    private SceneAsset sceneToView;

    [MenuItem("Window/Scene Viewer")]
    public static void ShowWindow()
    {
        GetWindow<CustomSceneWindow>("Scene Viewer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a scene to view:");

        sceneToView = EditorGUILayout.ObjectField(sceneToView, typeof(SceneAsset), false) as SceneAsset;

        if (GUILayout.Button("View Scene"))
        {
            if (sceneToView != null)
            {
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneToView));
            }
        }
    }
}