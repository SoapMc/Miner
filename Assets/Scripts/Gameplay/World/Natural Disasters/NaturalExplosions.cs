using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Exceptions;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Natural Disasters/Natural Explosions")]
    public class NaturalExplosions : NaturalDisaster
    {
        [SerializeField] private NaturalExplosionsGenerator _naturalExplosionsGeneratorPrefab = null;
        private NaturalExplosionsGenerator _naturalExplosionsGeneratorInstance = null;

        public override void End()
        {
            if (_naturalExplosionsGeneratorInstance != null)
            {
                Destroy(_naturalExplosionsGeneratorInstance.gameObject);
                _naturalExplosionsGeneratorInstance = null;
            }
        }

        public override void Execute()
        {
            _naturalExplosionsGeneratorInstance = Instantiate(_naturalExplosionsGeneratorPrefab);
        }
    }
}