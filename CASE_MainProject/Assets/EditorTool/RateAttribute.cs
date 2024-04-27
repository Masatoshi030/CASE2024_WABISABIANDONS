using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class RateAttribute : PropertyAttribute
{
    public readonly float fmaxRate;
    public readonly int imaxRate;

    public RateAttribute(float _rate)
    {
        this.fmaxRate = _rate;
        this.imaxRate = (int)_rate;
    }

    public RateAttribute(int _rate)
    {
        this.imaxRate = _rate;
        this.fmaxRate = (float)_rate;
    }
}
