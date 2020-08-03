using System;

[Serializable]
public struct RangedFloat
{
    public float minValue;
    public float maxValue;

    public RangedFloat(float minVal, float maxVal)
    {
        minValue = minVal;
        maxValue = maxVal;
    }

    public bool IsInRange(float value)
    {
        if (value >= minValue && value <= maxValue)
            return true;
        return false;
    }
}