using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using UnityEngine.Tilemaps;

namespace Miner.Gameplay
{
    public class PlayerRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer = default;
        [SerializeField] private Transform _rightSource = null;
        [SerializeField] private Transform _downSource = null;
        [SerializeField] private Transform _leftSource = null;
        private bool _canDigRight = false;
        private bool _canDigDown = false;
        private bool _canDigLeft = false;

        public bool CanDigRight => _canDigRight;
        public bool CanDigDown => _canDigDown;
        public bool CanDigLeft => _canDigLeft;

        private bool Raycast(Transform source, Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(source.position, direction, 0.05f, _groundLayer);
            Debug.DrawRay(source.position, 0.05f * direction, Color.red);
            if (hit.collider != null)
            {
                if(!hit.collider.isTrigger)
                    return true;
            }

            return false;     
        }

        private void Update()
        {
            _canDigRight = Raycast(_rightSource, transform.localScale.x * Vector2.right);
            _canDigDown = Raycast(_downSource, Vector2.down);
            _canDigLeft = Raycast(_leftSource, transform.localScale.x * Vector2.left);
        }

    }
}