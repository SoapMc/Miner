using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Cargo Reference Part")]
    public class CargoReferencePart : ReferencePart
    {
        [SerializeField] private int _volume = 0;

        public int Volume => _volume;
    }
}