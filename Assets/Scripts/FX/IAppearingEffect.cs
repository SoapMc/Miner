using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.FX
{
    public interface IAppearingEffect
    {
        event Action AppearingFinished;
        event Action DisappearingFinished;

        void TriggerAppearing();
        void TriggerDisappearing();
    }
}