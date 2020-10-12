using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public interface IOverheatable
    {
        /// <summary>
        /// Time in seconds when the part can operate in overheating without harm.
        /// </summary>
        float ToleranceTime { get; }

        /// <summary>
        /// Maximum temperature at which the part can operate constantly without harm.
        /// </summary>
        int MaximumOperatingTemperature { get; }
    }
}