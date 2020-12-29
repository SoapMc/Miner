using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.UI
{
    public class HUDController : MonoBehaviour
    {
        public void ResetHUD()
        {
            IResettableHUDComponent[] resettableComponents = GetComponentsInChildren<IResettableHUDComponent>();
            foreach(var resettable in resettableComponents)
            {
                resettable.ResetComponent();
            }
        }
    }
}