using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using UnityEngine.Tilemaps;
using System.Linq;

namespace Miner.Gameplay
{
    public class PlayerRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer = default;
        [SerializeField] private Transform _rightSource = null;
        [SerializeField] private Transform _downSource = null;
        [SerializeField] private Transform _leftSource = null;
        private bool _canDigRight = false;
        private bool _isGrounded = false;
        private bool _canDigLeft = false;

        public bool CanDigRight => _canDigRight;
        public bool IsGrounded => _isGrounded;
        public bool CanDigLeft => _canDigLeft;

        private bool Raycast(Transform source, Vector2 direction)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(source.position, direction, 0.05f, _groundLayer);
            Debug.DrawRay(source.position, 0.05f * direction, Color.red);
            if (hits.Any(x => !x.collider.isTrigger))
            {
                return true;
            }
            return false;     
        }

        public void UpdateRaycasts()
        {
            _canDigRight = Raycast(_rightSource, transform.localScale.x * Vector2.right);
            _isGrounded = Raycast(_downSource, Vector2.down);
            _canDigLeft = Raycast(_leftSource, transform.localScale.x * Vector2.left);
        }

    }
}