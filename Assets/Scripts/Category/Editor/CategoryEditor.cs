using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Category))]
public class CategoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Category category = (Category)target;

        if (category.material == null)
        {
            EditorGUILayout.HelpBox("Material cannot be set to null!", MessageType.Error);
        }
    }
}