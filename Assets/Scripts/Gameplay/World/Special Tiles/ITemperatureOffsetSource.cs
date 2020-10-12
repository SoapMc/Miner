using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITemperatureOffsetSource
{
    float GetTemperatureOffset(float sqrDistance);
}
