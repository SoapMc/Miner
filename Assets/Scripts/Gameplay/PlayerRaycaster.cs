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
        [SerializeField] private BoolReference _canDigRight = null;
        [SerializeField] private BoolReference _canDigDown = null;
        [SerializeField] private BoolReference _canDigLeft = null;

        private bool Raycast(Transform source, Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(source.position, direction, 0.1f, _groundLayer);
            Debug.DrawRay(source.position, direction, Color.red);
            if (hit.collider != null)
            {
                return true;
            }

            return false;     
        }

        private void Update()
        {
            _canDigRight.Value = Raycast(_rightSource, Vector2.right);
            _canDigDown.Value = Raycast(_downSource, Vector2.down);
            _canDigLeft.Value = Raycast(_leftSource, Vector2.left);
        }

    }
}