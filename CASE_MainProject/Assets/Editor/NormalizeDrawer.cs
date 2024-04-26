using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor.Rendering;
using Codice.CM.Client.Differences;


#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NormalizeAttribute))]
public class NormalizeDrawer : PropertyDrawer
{
    const float pad = 15.0f;
    const float windowHeight = 100.0f;
    const float LineWidth = 3.0f;

    static string[] names = { "FromRight", "FromUp", "FromBack" };
    enum Direction
    {FromX, FromY, FromZ};
    Direction direction = Direction.FromZ;

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

        Rect Window = new Rect(position.x, position.y + pad * 3, position.width, windowHeight);

        Vector2 start = new Vector2(Window.position.x + Window.width / 2, Window.position.y + Window.height / 2);
        Vector2 end = start;
        Vector2 Add = value * Window.height / 3;
        end.x += Add.x;
        end.y -= Add.y;
        EditorGUI.DrawRect(Window, Color.black);
        Handles.color = Color.white;
        Handles.DrawLine(start, end);
        Handles.color = Color.red;
        Vector2 start2 = start;
        start2.x += value.x * LineWidth;
        start2.y -= value.y * LineWidth;
        Handles.DrawLine(start, start2);
        Handles.color = Color.green;
        Vector2 end2 = end;
        end.x -= value.x * LineWidth;
        end.y += value.y * LineWidth;
        Handles.DrawLine(end, end2);

        Rect WindowLabelUp = new Rect(Window.x + Window.width / 2, Window.y, pad, pad);
        Rect WindowLabelRight = new Rect(Window.width - pad, Window.y + Window.height / 2, pad, pad);
        EditorGUI.LabelField(WindowLabelUp, "Y");
        EditorGUI.LabelField(WindowLabelRight, "X");

        if (EditorGUI.EndChangeCheck())
        {
            property.vector2Value = value;
        }

        GUILayout.Space(windowHeight);
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

        Rect Window = new Rect(position.x, position.y + pad * 3, position.width, windowHeight);

        Vector2 start = new Vector2(Window.x + Window.width / 2, Window.position.y + Window.height / 2);
        Vector2 end = start;

        Vector3 Add = value * Window.height / 3;

        Vector2 nor = Vector2.zero;

        switch (direction)
        {
            case Direction.FromX: end.x += Add.z;end.y -= Add.y; nor.x = value.z; nor.y = value.y; break;
            case Direction.FromY: end.x += Add.x;end.y -= Add.z; nor.x = value.x; nor.y = value.z; break;
            case Direction.FromZ: end.x += Add.x;end.y -= Add.y; nor.x = value.x; nor.y = value.y; break;
        }
        nor.Normalize();
        EditorGUI.DrawRect(Window, Color.black);
        Handles.color = Color.white;
        Handles.DrawLine(start, end);
        Handles.color = Color.red;
        Vector2 start2 = start;
        start2.x += nor.x * LineWidth;
        start2.y -= nor.y * LineWidth;
        Handles.DrawLine(start, start2);
        Handles.color = Color.green;
        Vector2 end2 = end;
        end.x -= nor.x * LineWidth;
        end.y += nor.y * LineWidth;
        Handles.DrawLine(end, end2);
        Rect WindowLabelUp = new Rect(Window.x + Window.width / 2, Window.y, pad, pad);
        Rect WindowLabelRight = new Rect(Window.width - pad, Window.y + Window.height / 2, pad, pad);

        switch (direction)
        {
            case Direction.FromX:
                EditorGUI.LabelField(WindowLabelUp, "Y");
                EditorGUI.LabelField(WindowLabelRight, "Z");
                break;
            case Direction.FromY:
                EditorGUI.LabelField(WindowLabelUp, "Z"); 
                EditorGUI.LabelField(WindowLabelRight, "X"); 
                break;
            case Direction.FromZ:
                EditorGUI.LabelField(WindowLabelUp, "Y");
                EditorGUI.LabelField(WindowLabelRight, "X");
                break;
        }

        float pad2 = windowHeight + pad * 4;
        Rect button = new Rect(position.x, position.y + pad2, position.width, position.height);
        int buttonvalue = GUI.Toolbar(button, ((int)direction), names);

        if (EditorGUI.EndChangeCheck())
        {
            property.vector3Value = value;
            direction = (Direction)buttonvalue;
        }

        GUILayout.Space(pad2);
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