using DocumentFormat.OpenXml.Drawing;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SnappingPointAttribute), true)]
[CustomPropertyDrawer(typeof(SnappingPoint))]
public class SnappingPointDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty building = property.FindPropertyRelative("Building");
        SerializedProperty room = property.FindPropertyRelative("Room");
        SerializedProperty posX = property.FindPropertyRelative("Position").FindPropertyRelative("X");
        SerializedProperty posY = property.FindPropertyRelative("Position").FindPropertyRelative("Y");
        SerializedProperty posZ = property.FindPropertyRelative("Position").FindPropertyRelative("Z");
        SerializedProperty rotX = property.FindPropertyRelative("Rotation").FindPropertyRelative("X");
        SerializedProperty rotY = property.FindPropertyRelative("Rotation").FindPropertyRelative("Y");
        SerializedProperty rotZ = property.FindPropertyRelative("Rotation").FindPropertyRelative("Z");
        SerializedProperty url = property.FindPropertyRelative("Url");

        EditorGUILayout.LabelField("Snapping Point", EditorStyles.boldLabel);

        building.stringValue = CreateHorizontal("Building", building.stringValue, 80, 170);
        room.stringValue = CreateHorizontal("Room", room.stringValue, 80, 170);

        SnappingPointAttribute snappingPointAttribute = (SnappingPointAttribute)attribute;
        bool positionEditable = true;
        bool rotationEditable = true;

        if (snappingPointAttribute != null)
        {
            positionEditable = snappingPointAttribute.positionEditable;
            rotationEditable = snappingPointAttribute.rotationEditable;
        }

        GUI.enabled = positionEditable;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Position");
        GUILayout.FlexibleSpace();
        posX.floatValue = DraggableFloatField("X", posX.floatValue);
        posY.floatValue = DraggableFloatField("Y", posY.floatValue);
        posZ.floatValue = DraggableFloatField("Z", posZ.floatValue);
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        GUI.enabled = rotationEditable;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Rotation");
        GUILayout.FlexibleSpace();
        rotX.floatValue = DraggableFloatField("X", rotX.floatValue);
        rotY.floatValue = DraggableFloatField("Y", rotY.floatValue);
        rotZ.floatValue = DraggableFloatField("Z", rotZ.floatValue);
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        url.stringValue = CreateHorizontal("URL", url.stringValue, 80, 170);
        EditorGUI.EndProperty();

    }

    private float DraggableFloatField(string label, float value)
    {
        EditorGUIUtility.labelWidth = 10;
        value = EditorGUILayout.FloatField(label, value, GUILayout.Width(60));
        return value;
    }

    private T CreateHorizontal<T>(string label, T value, float labelWidth, float fieldWidth)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(label, GUILayout.Width(labelWidth));
        GUILayout.FlexibleSpace();
        if (typeof(T) == typeof(string))
        {
            value = (T)(object)EditorGUILayout.TextField((string)(object)value, GUILayout.Width(fieldWidth));
        }
        else if (typeof(T) == typeof(int))
        {
            value = (T)(object)EditorGUILayout.IntField((int)(object)value, GUILayout.Width(fieldWidth));

        }
        else if (typeof(T) == typeof(float))
        {
            value = (T)(object)EditorGUILayout.FloatField((float)(object)value, GUILayout.Width(fieldWidth));
        }

        EditorGUILayout.EndHorizontal();

        return value;
    }
}
