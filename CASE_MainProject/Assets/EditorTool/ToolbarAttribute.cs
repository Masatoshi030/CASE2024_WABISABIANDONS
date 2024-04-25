using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ToolbarAttribute : PropertyAttribute
{
    // ������i�[�p
    public readonly string[] labels;
    public readonly Type type;
    // �R�[���o�b�N��(�I�����Ɋ֐����Ăяo����)
    public readonly string method;

    public ToolbarAttribute(string[] labels, string method = null)
    {
        this.labels = labels;
        type = null;
        this.method = method;
    }

    public ToolbarAttribute(Type type, string method = null)
    {
        labels = null;
        this.type = type;
        this.method = method;
    }
}
