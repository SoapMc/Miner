using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Exceptions;
using Miner.Management.Events;
using Miner.Management;
using NewWorldCreatedEA = Miner.Management.Events.WorldLoadedEA;
using System.Linq;

namespace Miner.Gameplay
{
    public class PositionController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Range(10f, 100f)] private float _maxSpeed = 50f;

        [Header("Resources")]
        [SerializeField] private PlayerController _playerController = null;
        [SerializeField] private FloatReference _playerLayerRelativePosition = null;
        [SerializeField] private IntReference _playerLayer = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private Vector2Reference _playerCurrentSpeed = null;
        [SerializeField] private GroundLayerList _layers = null;
        private Grid _worldGrid = null;
        private Rigidbody2D _rigidbody = null;
        private float _offsetY_playerCenter_cellCenter = 0f;
        private float _visitedLayerDepth = 1f;
        private float _layerTopDepth = 0f;

        [Header("Events")]
        [SerializeField] private GameEvent _playerTranslated = null;
        [SerializeField] private GameEvent _digCompleted = null;

        public void OnAllowDig(EventArgs args)
        {
            if (args is AllowDigEA ltdp)
            {
                StartCoroutine(FollowToDigPlace(ltdp.Place, ltdp.Speed, ltdp.Direction));
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                _playerLayer.Value = pctl.GroundLayer.LayerNumber;
                if(pctl.GroundLayer.Depth > 0)
                    _visitedLayerDepth = pctl.GroundLayer.Depth;
                _layerTopDepth = CalculateTopDepth(pctl.GroundLayer.LayerNumber);
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnTranslatePlayer(EventArgs args)
        {
            if (args is TranslatePlayerEA mp)
            {
                Vector2 oldPosition = transform.position;
                Vector2Int oldGridPosition = _playerGridPosition.Value;
                _playerController.transform.position = mp.Position;
                _playerGridPosition.Value = (Vector2Int)_worldGrid.WorldToCell(transform.position);
                _playerTranslated.Raise(new PlayerTranslatedEA(oldPosition, transform.position, oldGridPosition, _playerGridPosition.Value));
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnWorldLoaded(EventArgs args)
        {
            if (args is WorldLoadedEA wl)
            {
                _worldGrid = wl.WorldGrid;
                OnTranslatePlayer(new TranslatePlayerEA(wl.PlayerSpawnPoint.position));
                enabled = true;
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnNewWorldCreated(EventArgs args)
        {
            if (args is NewWorldCreatedEA nwc)
            {
                _worldGrid = nwc.WorldGrid;
                OnTranslatePlayer(new TranslatePlayerEA(nwc.PlayerSpawnPoint.position));
                enabled = true;
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        private float CalculateHeightFromGround()
        {
            CircleCollider2D[] tracksColliders = _playerController.GetComponentsInChildren<CircleCollider2D>();
            CircleCollider2D lowerCollider = tracksColliders.OrderByDescending(x => x.transform.localPosition.y - x.radius).Last();
            BoxCollider2D mainCollider = _playerController.GetComponent<BoxCollider2D>();
            return lowerCollider.radius + (-lowerCollider.transform.localPosition.y - mainCollider.size.y / 2f) + mainCollider.size.y / 2f;
        }

        private float CalculateTopDepth(int layerNumber)
        {
            if (layerNumber >= _layers.Count) throw new ArgumentOutOfRangeException();

            float result = 0f;
            for (int i = 0; i < layerNumber; ++i)
            {
                result += _layers[i].Depth;
            }
            return result;
        }

        #region COROUTINES
        public IEnumerator FollowToDigPlace(Vector2Int coords, float speed, EDigDirection direction)
        {
            _playerController.Locked = true;
            Vector3 worldCoords = _worldGrid.GetCellCenterWorld((Vector3Int)coords);
            if (direction == EDigDirection.Left || direction == EDigDirection.Right)
                worldCoords += new Vector3(0, _offsetY_playerCenter_cellCenter, 0);

            float lerpCoeff = 0f;
            Vector3 startPosition = transform.position;
            _rigidbody.simulated = false;

            while (lerpCoeff <= 1f)
            {
                lerpCoeff += speed * Time.deltaTime;
                _playerController.transform.position = Vector3.Lerp(startPosition, worldCoords, lerpCoeff);
                yield return null;
            }

            _digCompleted.Raise();
            _rigidbody.simulated = true;
            _playerController.Locked = false;
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            enabled = false;
            _offsetY_playerCenter_cellCenter = -0.5f + CalculateHeightFromGround();
            _rigidbody = _playerController.GetComponent<Rigidbody2D>();
            _playerLayer.Value = -1;
        }

        private void Update()
        {
            _playerCurrentSpeed.Value = _rigidbody.velocity;
            _playerGridPosition.Value = (Vector2Int)_worldGrid.WorldToCell(_playerController.transform.position);
            _playerPosition.Value = _playerController.transform.position;
            _playerLayerRelativePosition.Value = Mathf.Clamp01(Mathf.Abs(-_layerTopDepth - _playerPosition.Value.y)/_visitedLayerDepth );
            if (_rigidbody.velocity.magnitude > _maxSpeed)
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
            _playerController.transform.rotation = Quaternion.Euler(0, 0, -_rigidbody.velocity.x);
        }
        #endregion
    }
}