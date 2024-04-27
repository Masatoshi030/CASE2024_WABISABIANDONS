using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.Rendering.UI;
using NUnit.Framework.Constraints;
using UnityEngine.UIElements;





#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AnimCurveAttribute))]

public class AnimCurveDrawer : PropertyDrawer
{
    // �l���͕ϐ�
    public float time = 0.0f;
    public float value = 0.0f;

    // ���s�p�ϐ�
    const float pad = 20.0f;
    const float curveHeight = pad * 3;

    // ���X�g�̕\��bool
    bool bFoldList = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();

        if(property.type == "AnimationCurve")
        {
            DrawCurve(position, property, label);
        }
        else
        {

        }

        EditorGUI.EndProperty();
    }

    void DrawCurve(Rect position, SerializedProperty property, GUIContent label)
    {
        int index = 0;
        AnimCurveAttribute att = attribute as AnimCurveAttribute;
        AnimationCurve curve = property.animationCurveValue;
        Rect CurveRect = new Rect(position.x, position.y, position.width, curveHeight);
        curve = EditorGUI.CurveField(CurveRect, curve);
        index += 3;

        Rect ListRect = new Rect(position.x, position.y + pad * index, position.width, position.height);
        index++;
        bFoldList = EditorGUI.Foldout(ListRect, bFoldList, "Keys");
        if (bFoldList)
        {
            EditorGUI.indentLevel++;
            for(int i = 0; i < curve.keys.Length; i++)
            {
                Rect cTimeLabel = new Rect(position.x, position.y + pad * index, position.width / 5, position.height);
                Rect cTimeValue = new Rect(position.x + position.width/5, position.y + pad * index, position.width / 5, position.height);
                Rect cValueLabel = new Rect(position.x + position.width/5 * 2, position.y + pad * index, position.width / 5, position.height);
                Rect cValueValue = new Rect(position.x + position.width/5 * 3, position.y + pad * index, position.width / 5, position.height);
                Rect cDeleteRect = new Rect(position.x + position.width / 5 * 4, position.y + pad * index, position.width / 5, position.height);
                
                EditorGUI.LabelField(cTimeLabel, "Time");
                float t = EditorGUI.FloatField(cTimeValue, curve.keys[i].time);
                EditorGUI.LabelField(cValueLabel, "Value");
                float v = EditorGUI.FloatField(cValueValue, curve.keys[i].value);

                // �L�[�̃R�s�[
                Keyframe key = curve.keys[i];
                // ���͒l�̔��f
                key.time = t;
                key.value = v;
                curve.MoveKey(i, key);

                // �L�[�̍폜����
                if (GUI.Button(cDeleteRect, "Delete"))
                {
                    curve.RemoveKey(i);
                }
                index++;
            }
            EditorGUI.indentLevel--;
        }

        index++;

        Rect TimeLabel = new Rect(position.x, position.y + pad * index, position.width / 4, position.height);
        Rect ValueLabel = new Rect(position.x + position.width / 2, position.y + pad * index, position.width / 4, position.height);
        Rect TimeRect = new Rect(position.x + position.width / 4, position.y + pad * index, position.width / 4, position.height);
        Rect ValueRect = new Rect(position.x + position.width / 4 * 3, position.y + pad * index, position.width / 4, position.height);
        EditorGUI.LabelField(TimeLabel, "Time");
        EditorGUI.LabelField(ValueLabel, "Value");
        time = EditorGUI.FloatField(TimeRect, time);
        value = EditorGUI.FloatField(ValueRect, value);
        index++;

        Rect ButtonRect = new Rect(position.x, position.y + pad * index, position.width, position.height);

        // �L�[�̒ǉ�����
        if (GUI.Button(ButtonRect, "Add Key"))
        {
            curve.AddKey(time, value);
        }

        if(EditorGUI.EndChangeCheck())
        {
            property.animationCurveValue = curve;
        }
        GUILayout.Space(pad * 3 + pad * index);
    }
}
#endif