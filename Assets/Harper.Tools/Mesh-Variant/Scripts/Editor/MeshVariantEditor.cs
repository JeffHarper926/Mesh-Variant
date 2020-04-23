using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MeshVariant))]
public class MeshVariantEditor : Editor
{
    private MeshVariant meshVariant;

    private void OnEnable()
    {
        meshVariant = (MeshVariant)target;

        if (!meshVariant.meshFilter)
        {
            serializedObject.Update();
            meshVariant.meshFilter = meshVariant.GetComponent<MeshFilter>();
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void OnDisable()
    {
        if(EditorWindow.GetWindow<MeshVariantTextureWindow>())
            EditorWindow.GetWindow<MeshVariantTextureWindow>().Close();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("includeDefault"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("texture"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("variants"), true);
        if (GUILayout.Button("Add Default Variant"))
        {
            meshVariant.AddDefault();
        }

        serializedObject.ApplyModifiedProperties();
    }
}