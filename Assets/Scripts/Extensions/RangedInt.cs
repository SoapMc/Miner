using System;

[Serializable]
public struct RangedInt
{
    public int minValue;
    public int maxValue;

    public RangedInt(int minVal, int maxVal)
    {
        minValue = minVal;
        maxValue = maxVal;
    }

    public bool IsInRange(int value)
    {
        if (value >= minValue && value <= maxValue)
            return true;
        return false;
    }
}