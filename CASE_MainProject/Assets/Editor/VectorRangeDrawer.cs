using System;
using UnityEngine;
using Unity.VisualScripting;
using Codice.CM.Common;

#if UNITY_EDITOR
using UnityEditor;

#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(VectorRangeAttribute))]
public class VectorRangeDrawer : PropertyDrawer
{
    const float pad = 15.0f;
    const float widthSeparate = 2.0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        switch (property.type)
        {
            case "Vector2": Vector2Range(position, property, label); break;
            case "Vector3": Vector3Range(position, property, label); break;
            case "Vector2Int": Vector2IRange(position, property, label); break;
            case "Vector3Int": Vector3IRange(position, property, label); break;
        }
        GUILayout.Space(pad);
        EditorGUI.EndProperty();
    }

    void Vector2Range(Rect position, SerializedProperty property, GUIContent label)
    {
        VectorRangeAttribute range = attribute as VectorRangeAttribute;
        Vector2 value = property.vector2Value;

        Rect xLabel = new Rect(position.x, position.y, position.width / 2, position.height);
        Rect yLabel = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);
        Rect xSlider = new Rect(position.x, position.y + pad, position.width / widthSeparate, position.height);
        Rect ySlider = new Rect(position.x + position.width / 2, position.y + pad, position.width / widthSeparate, position.height);

        EditorGUI.LabelField(xLabel, "x");
        EditorGUI.LabelField(yLabel, "y");
        //Rect xSliderNoLabel = EditorGUI.PrefixLabel(xSlider, GUIUtility.GetControlID(FocusType.Passive), label);
        //Rect ySliderNoLabel = EditorGUI.PrefixLabel(ySlider, GUIUtility.GetControlID(FocusType.Passive), label);
        value.x = EditorGUI.Slider(xSlider, value.x, range.min, range.max);
        value.y = EditorGUI.Slider(ySlider, value.y, range.min, range.max);

        if (EditorGUI.EndChangeCheck())
        {
            property.vector2Value = value;
        }
    }

    void Vector3Range(Rect position, SerializedProperty property, GUIContent label)
    {
        VectorRangeAttribute range = attribute as VectorRangeAttribute;
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

        value.x = EditorGUI.Slider(xSlider, value.x, range.min, range.max);
        value.y = EditorGUI.Slider(ySlider, value.y, range.min, range.max);
        value.z = EditorGUI.Slider(zSlider, value.z, range.min, range.max);

        if (EditorGUI.EndChangeCheck())
        {
            property.vector3Value = value;
        }
    }

    void Vector2IRange(Rect position, SerializedProperty property, GUIContent label)
    {
        VectorRangeAttribute range = attribute as VectorRangeAttribute;
        Vector2Int value = property.vector2IntValue;

        Rect xLabel = new Rect(position.x, position.y, position.width / 2, position.height);
        Rect yLabel = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);
        Rect xSlider = new Rect(position.x, position.y + pad, position.width / widthSeparate, position.height);
        Rect ySlider = new Rect(position.x + position.width / 2, position.y + pad, position.width / widthSeparate, position.height);

        EditorGUI.LabelField(xLabel, "x");
        EditorGUI.LabelField(yLabel, "y");
        //Rect xSliderNoLabel = EditorGUI.PrefixLabel(xSlider, GUIUtility.GetControlID(FocusType.Passive), label);
        //Rect ySliderNoLabel = EditorGUI.PrefixLabel(ySlider, GUIUtility.GetControlID(FocusType.Passive), label);
        value.x = EditorGUI.IntSlider(xSlider, value.x, (int)range.min, (int)range.max);
        value.y = EditorGUI.IntSlider(ySlider, value.y, (int)range.min, (int)range.max);

        if (EditorGUI.EndChangeCheck())
        {
            property.vector2IntValue = value;
        }
    }

    void Vector3IRange(Rect position, SerializedProperty property, GUIContent label)
    {
        VectorRangeAttribute range = attribute as VectorRangeAttribute;
        Vector3Int value = property.vector3IntValue;

        Rect xLabel = new Rect(position.x, position.y, position.width / 3, position.height);
        Rect yLabel = new Rect(position.x + position.width / 3, position.y, position.width / 3, position.height);
        Rect zLabel = new Rect(position.x + position.width / 3 * 2, position.y, position.width / 3, position.height);
        Rect xSlider = new Rect(position.x, position.y + pad, position.width / 3, position.height);
        Rect ySlider = new Rect(position.x + position.width / 3, position.y + pad, position.width / 3, position.height);
        Rect zSlider = new Rect(position.x + position.width / 3 * 2, position.y + pad, position.width / 3, position.height);

        EditorGUI.LabelField(xLabel, "x");
        EditorGUI.LabelField(yLabel, "y");
        EditorGUI.LabelField(zLabel, "z");

        value.x = EditorGUI.IntSlider(xSlider, value.x, (int)range.min, (int)range.max);
        value.y = EditorGUI.IntSlider(ySlider, value.y, (int)range.min, (int)range.max);
        value.z = EditorGUI.IntSlider(zSlider, value.z, (int)range.min, (int)range.max);

        if (EditorGUI.EndChangeCheck())
        {
            property.vector3IntValue = value;
        }
    }
}
#endif
