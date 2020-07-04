using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerInputSheet _inputSheet = null;

        public PlayerInputSheet InputSheet => _inputSheet;
        private void Update()
        {
            _inputSheet.Update();
        }
    }
}