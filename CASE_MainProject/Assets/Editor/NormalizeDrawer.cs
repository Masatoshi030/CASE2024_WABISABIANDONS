using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NormalizeAttribute))]
public class NormalizeDrawer : PropertyDrawer
{
    const float pad = 15.0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();

        switch (property.type)
        {
            case "Vector2": NormalizeVector2(position, property, label); break;
            case "Vector3": NormalizeVector3(position, property, label); break;
            case "Vector4": NormalizeVector4(position, property, label); break;
        }
        GUILayout.Space(pad);
        EditorGUI.EndProperty();
    }

    void NormalizeVector2(Rect position, SerializedProperty property, GUIContent label)
    {
        NormalizeAttribute att = attribute as NormalizeAttribute;
        Vector2 value = property.vector2Value;

        Rect xLabel = new Rect(position.x, position.y, position.width / 2, position.height);
        Rect yLabel = new Rect(position.x + position.width/2, position.y, position.width / 2, position.height);
        Rect xSlider = new Rect(position.x, position.y + pad, position.width / 2, position.height);
        Rect ySlider = new Rect(position.x + position.width / 2, position.y + pad, position.width / 2, position.height);

        EditorGUI.LabelField(xLabel, "x");
        EditorGUI.LabelField(yLabel, "y");
        value.x = EditorGUI.Slider(xSlider, value.x, -1.0f, 1.0f);
        value.y = EditorGUI.Slider(ySlider, value.y, -1.0f, 1.0f);
        value.Normalize();

        if(EditorGUI.EndChangeCheck())
        {
            property.vector2Value = value;
        }
    }

    void NormalizeVector3(Rect position, SerializedProperty property, GUIContent label)
    {
        NormalizeAttribute att = attribute as NormalizeAttribute;
        Vector3 value = property.vector3Value;

        Rect xLabel = new Rect(position.x, position.y, position.width / 3, position.height);
        Rect yLabel = new Rect(position.x + position.width / 3, position.y, position.width / 3, position.height);
        Rect zLabel = new Rect(position.x + position.width / 3 * 2, position.y, position.width / 3, position.height);
        Rect xSlider = new Rect(position.x, position.y + pad, position.width / 3, position.height);
        Rect ySlider = new Rect(position.x + position.width / 3, position.y + pad, position.width / 3, position.height);
        Rect zSlider = new Rect(position.x + position.width / 3 * 2, position.y + pad, position.width / 3, position.height);

        EditorGUI.LabelField(xLabel, "x");
        EditorGUI.LabelField(yLabel, "y");
        EditorGUI.LabelField(zLabel, "z");
        value.x = EditorGUI.Slider(xSlider, value.x, -1.0f, 1.0f);
        value.y = EditorGUI.Slider(ySlider, value.y, -1.0f, 1.0f);
        value.z = EditorGUI.Slider(zSlider, value.z, -1.0f, 1.0f);
        value.Normalize();

        if (EditorGUI.EndChangeCheck())
        {
            property.vector3Value = value;
        }
    }

    void NormalizeVector4(Rect position, SerializedProperty property, GUIContent label)
    {
        NormalizeAttribute att = attribute as NormalizeAttribute;
        Vector4 value = property.vector4Value;

        Rect xLabel = new Rect(position.x, position.y, position.width / 2, position.height);
        Rect yLabel = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);
        Rect zLabel = new Rect(position.x, position.y + pad * 2, position.width / 2, position.height);
        Rect wLabel = new Rect(position.x + position.width/2, position.y + pad * 2, position.width / 2, position.height);
        Rect xSlider = new Rect(position.x, position.y + pad, position.width / 2, position.height);
        Rect ySlider = new Rect(position.x + position.width / 2, position.y + pad, position.width / 2, position.height);
        Rect zSlider = new Rect(position.x, position.y + pad * 3, position.width / 2, position.height);
        Rect wSlider = new Rect(position.x + position.width / 2, position.y + pad * 3, position.width / 2, position.height);

        EditorGUI.LabelField(xLabel, "x");
        EditorGUI.LabelField(yLabel, "y");
        EditorGUI.LabelField(zLabel, "z");
        EditorGUI.LabelField(wLabel, "w");
        value.x = EditorGUI.Slider(xSlider, value.x, -1.0f, 1.0f);
        value.y = EditorGUI.Slider(ySlider, value.y, -1.0f, 1.0f);
        value.z = EditorGUI.Slider(zSlider, value.z, -1.0f, 1.0f);
        value.w = EditorGUI.Slider(wSlider, value.w, -1.0f, 1.0f);
        value.Normalize();

        if (EditorGUI.EndChangeCheck())
        {
            property.vector4Value = value;
        }

        GUILayout.Space(pad * 2);
    }
}

#endif