using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MeshVariant.VariantUV.UVCoord))]
public class VariantUVDrawer : PropertyDrawer
{
    float totalHeight = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty currentProp = property.FindPropertyRelative("currentUV");
        SerializedProperty newProp = property.FindPropertyRelative("newUV");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float currentHeight = EditorGUI.GetPropertyHeight(currentProp);
        float newHeight = EditorGUI.GetPropertyHeight(currentProp);

        SerializedProperty texture = property.serializedObject.FindProperty("texture");
        if (texture.objectReferenceValue)
        {
            if (texture.objectReferenceValue.GetType() == typeof(Texture2D))
            {
                Texture2D tex = (Texture2D)texture.objectReferenceValue;

                Texture2D currentColor = CreateColorTexture(tex.GetPixel(currentProp.vector2IntValue.x, currentProp.vector2IntValue.y));
                GUI.DrawTexture(new Rect(position.x + 30, position.y + 13, lineHeight - 5, lineHeight - 5), currentColor);

                Texture2D newColor = CreateColorTexture(tex.GetPixel(newProp.vector2IntValue.x, newProp.vector2IntValue.y));
                GUI.DrawTexture(new Rect(position.x + 30, position.y + currentHeight + 18, lineHeight - 5, lineHeight - 5), newColor);
            }
        }

        EditorGUI.PropertyField(new Rect(position.x, position.y + 10, position.width - 70, currentHeight), currentProp);
        if (GUI.Button(new Rect(position.width - 60, position.y + 10, 70, lineHeight), "Pick Color"))
        {
            OpenTextureWindow(currentProp);
        }

        EditorGUI.PropertyField(new Rect(position.x, position.y + currentHeight + 15, position.width - 70, newHeight), newProp);
        if (GUI.Button(new Rect(position.width - 60, position.y + currentHeight + 15, 70, lineHeight), "Pick Color"))
        {
            OpenTextureWindow(newProp);
        }

        totalHeight = currentHeight + newHeight;
    }

    public void OpenTextureWindow(SerializedProperty property)
    {
        MeshVariantTextureWindow window = new MeshVariantTextureWindow();
        window.ShowUtility();
        window.prop = property;
    }

    private Texture2D CreateColorTexture(Color color)
    {
        Texture2D newTex = new Texture2D(2, 2);
        for (int i = 0; i < newTex.width; i++)
        {
            for (int j = 0; j < newTex.height; j++)
            {
                newTex.SetPixel(i, j, color);
            }
        }
        newTex.Apply();
        return newTex;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return totalHeight + 20;
    }
}
