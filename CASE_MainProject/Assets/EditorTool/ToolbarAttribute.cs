using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ToolbarAttribute : PropertyAttribute
{
    // 文字列格納用
    public readonly string[] labels;
    public readonly Type type;
    // コールバック名(選択時に関数を呼び出せる)
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
