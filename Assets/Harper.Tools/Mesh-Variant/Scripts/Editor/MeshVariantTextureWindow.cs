using UnityEngine;
using UnityEditor;

public class MeshVariantTextureWindow : EditorWindow
{
    public Texture2D texture;
    public SerializedProperty prop;

    private void OnGUI()
    {
        texture = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), true);

        if (texture && texture.width <= 16 && texture.height <= 16)
        {
            for (int y = texture.height - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < texture.width; x++)
                {
                    if (GUILayout.Button(CreateColorTexture(texture.GetPixel(x, y)), GUILayout.Width(30), GUILayout.Height(30)))
                    {
                        prop.serializedObject.Update();
                        prop.vector2IntValue = new Vector2Int(x, y);
                        prop.serializedObject.ApplyModifiedProperties();
                        GetWindow<MeshVariantTextureWindow>().Close();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Missing Texture or Texture is too Large. Please make sure that the texture is less than or equal to 16 x 16.", MessageType.Warning);
        }
    }

    private Texture2D CreateColorTexture(Color color)
    {
        Texture2D newTex = new Texture2D(30, 30);
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
}