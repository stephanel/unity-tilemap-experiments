using UnityEngine;

public class HeatmapGridObject
{
    public const int Min = 0;
    public const int Max = 100;

    public int value { get; private set; }

    public void addValue(int valueToAdd)
    {
        value += valueToAdd;
        Mathf.Clamp(value, Min, Max);
    }

    public float GetNormalizedValue()
    {
        return (float)value / Max;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
