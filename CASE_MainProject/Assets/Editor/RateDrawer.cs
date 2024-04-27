using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(RateAttribute))]
public class RateDrawer : PropertyDrawer
{
    const float pad = 20.0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);

        switch (property.type)
        {
            case "float": FloatFunc(position, property, label); break;
            case "int": IntFunc(position, property, label); break;
        }

        EditorGUI.EndProperty();
    }

    void FloatFunc(Rect position, SerializedProperty property, GUIContent label)
    {
        RateAttribute rate = attribute as RateAttribute;
        float maxRate = rate.fmaxRate;
        float value = property.floatValue;
        int index = 0;
        Rect ValueRect = new Rect(position.x, position.y, position.width, position.height);
        value = EditorGUI.Slider(ValueRect, label, value, 0.0f, maxRate);
        index++;
        Rect BarRect = new Rect(position.x, position.y + pad * index, position.width, position.height);
        float RateValue = value / maxRate;
        float textValue = RateValue * 100.0f;
        string text = textValue.ToString() + "%";
        EditorGUI.ProgressBar(BarRect, RateValue, text);

        GUILayout.Space(pad * index);
        if(EditorGUI.EndChangeCheck())
        {
            property.floatValue = value;
        }
    }
    void IntFunc(Rect position, SerializedProperty property, GUIContent label)
    {
        RateAttribute rate = attribute as RateAttribute;
        int maxRate = rate.imaxRate;
        int value = property.intValue;
        int index = 0;

        Rect ValueRect = new Rect(position.x, position.y, position.width, position.height);
        value = EditorGUI.IntSlider(ValueRect, label, value, 0, maxRate);
        index++;
        Rect BarRect = new Rect(position.x, position.y + pad * index, position.width, position.height);
    }
}
#endif