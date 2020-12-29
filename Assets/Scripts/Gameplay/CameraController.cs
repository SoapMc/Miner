using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using NewPlayerCreatedEA = Miner.Management.Events.PlayerLoadedEA;
using Miner.Management;

namespace Miner.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1f)] private float _rigidity = 2f;
        [SerializeField] private Transform _target = null;
        [SerializeField] private WorldInfo _worldInfo = null;
        [SerializeField] private Vector2IntReference _cameraGridPosition = null;
        [SerializeField] private GameEvent _cameraTranslated = null;
        private Camera _camera = null;
        private Grid _worldGrid = null;
        private float _topBorder = 0f;
        private float _bottomBorder = 0f;
        private float _leftBorder = 0f;
        private float _rightBorder = 0f;

        private bool _worldInstantiated = false;
        private bool _playerInstantiated = false;

        

        public void OnWorldLoaded(EventArgs args)
        {
            if (args is WorldLoadedEA wl)
            {
                if (_worldGrid == null)
                    _worldGrid = wl.WorldGrid;
                _leftBorder = _worldInfo.HorizontalBorders.x * _worldGrid.cellSize.x + _camera.orthographicSize * Screen.width / Screen.height;
                _rightBorder = _worldInfo.HorizontalBorders.y * _worldGrid.cellSize.x - _camera.orthographicSize * Screen.width / Screen.height;
                _topBorder = _worldInfo.VerticalBorders.x * _worldGrid.cellSize.y - _camera.orthographicSize;
                _bottomBorder = _worldInfo.VerticalBorders.y * _worldGrid.cellSize.y + _camera.orthographicSize;
                _worldInstantiated = true;
                EnableBehaviourUnderCircumstances();
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
            
        }

        public void OnTranslatePlayer(EventArgs args)
        {
            if(args is TranslatePlayerEA mp)
            {
                Vector2Int oldGridPos = _cameraGridPosition.Value;
                SetAtPosition(mp.Position);
                _cameraTranslated.Raise(new CameraTranslatedEA(oldGridPos, _cameraGridPosition.Value));
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnPlayerLoaded(EventArgs args)
        {
            if(args is PlayerLoadedEA pl)
            {
                _target = pl.Player.transform;
                if (_worldGrid == null)
                    _worldGrid = FindObjectOfType<Grid>();
                _playerInstantiated = true;
                EnableBehaviourUnderCircumstances();
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnNewPlayerCreated(EventArgs args)
        {
            if (args is NewPlayerCreatedEA npc)
            {
                _target = npc.Player.transform;
                if (_worldGrid == null)
                    _worldGrid = FindObjectOfType<Grid>();
                _playerInstantiated = true;
                EnableBehaviourUnderCircumstances();
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnPlayerReset()
        {
            _target = null;
            _playerInstantiated = false;
            enabled = false;
        }

        public void OnWorldReset()
        {
            _worldGrid = null;
            _worldInstantiated = false;
            enabled = false;
        }

        private void SetAtPosition(Vector2 pos)
        {
            float x = Mathf.Clamp(pos.x, _leftBorder, _rightBorder);
            float y = Mathf.Clamp(pos.y, _bottomBorder, _topBorder);
            transform.position = new Vector3(x, y, _camera.transform.position.z);
            _cameraGridPosition.Value = (Vector2Int)_worldGrid.WorldToCell(transform.position);
        }

        private void EnableBehaviourUnderCircumstances()
        {
            if (_worldInstantiated && _playerInstantiated)
            {
                SetAtPosition(_target.position);
                enabled = true;
            }
            else
                enabled = false;
        }

        void Awake()
        {
            _camera = GetComponent<Camera>();
            enabled = false;
        }

        void FixedUpdate()
        {
            Vector2 vec = Vector3.Lerp(transform.position, _target.position, _rigidity);
            SetAtPosition(vec);
        }
    }


}