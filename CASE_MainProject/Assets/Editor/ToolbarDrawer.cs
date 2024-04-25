using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR

using UnityEditor;

#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ToolbarAttribute))]
public class ToolbarDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // プロパティのエディタレイアウトの設定
        label = EditorGUI.BeginProperty(position, label, property);

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer: IntergerToolbar(position, property, label); break;
            case SerializedPropertyType.Enum: EnumToolbar(position, property, label); break;
        }
        // 設定終了
        EditorGUI.EndProperty();
    }

    void IntergerToolbar(Rect position, SerializedProperty property, GUIContent label)
    {
        ToolbarAttribute toolbar = attribute as ToolbarAttribute;
        EditorGUI.BeginChangeCheck();

        int newIndex = GUI.Toolbar(position, property.intValue, toolbar.labels);

        if(EditorGUI.EndChangeCheck())
        {
            property.intValue = newIndex;
            InvokeMethod(property, newIndex);
        }
    }

    void EnumToolbar(Rect position, SerializedProperty property, GUIContent label)
    {
        ToolbarAttribute toolbar = attribute as ToolbarAttribute;
        EditorGUI.BeginChangeCheck();

        int newIndex = GUI.Toolbar(position, property.enumValueIndex, GetInspectorNames());

        if(EditorGUI.EndChangeCheck())
        {
            property.enumValueIndex = newIndex;
            InvokeMethod(property, newIndex);
        }
    }

    void InvokeMethod(SerializedProperty property, object parameter)
    {
        ToolbarAttribute toolbar = attribute as ToolbarAttribute;
        if(!string.IsNullOrEmpty(toolbar.method))
        {
            object obj = property.serializedObject.targetObject;
            MethodInfo method = obj.GetType().GetMethod(toolbar.method, BindingFlags.Public | BindingFlags.Instance);
            method.Invoke(obj, new[] { parameter });
        }
    }

    string[] GetInspectorNames()
    {
        ToolbarAttribute toolbar = attribute as ToolbarAttribute;
        Dictionary<string, string> result = new Dictionary<string, string>();
        string[] names = toolbar.type.GetEnumNames();
        foreach(string name in names)
        {
            result.Add(name, name);
        }

        FieldInfo[] fields = toolbar.type.GetFields();
        foreach(FieldInfo field in fields)
        {
            var inspectorName = Attribute.GetCustomAttribute(field, typeof(InspectorNameAttribute)) as InspectorNameAttribute;
            if(inspectorName != null)
            {
                if(result.ContainsKey(field.Name))
                {
                    result[field.Name] = inspectorName.displayName;
                }
            }
        }
        return result.Values.ToArray();
    }
}
#endif