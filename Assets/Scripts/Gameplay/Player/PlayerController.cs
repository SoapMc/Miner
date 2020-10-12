using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.FX;
using Miner.UI;

namespace Miner.Gameplay
{
    [RequireComponent(typeof(PlayerRaycaster), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private IntReference _playerEnginePower = null;
        [SerializeField] private Vector2IntReference _gridPosition = null;
        [SerializeField] private Vector2Reference _currentSpeed = null;
        [SerializeField] private FloatReference _drillSharpness = null;
        [SerializeField] private IntReference _playerCargoMass = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private ParticleSystem _rocket = null;
        [SerializeField] private PlayerInputSheet _input = null;
        [SerializeField, Range(10f, 100f)] private float _maxSpeed = 25f;
        [SerializeField] private Light _light = null;
        [SerializeField] private IntReference _playerLayer = null;
        [SerializeField] private Window _inventoryWindowPrefab = null;

        [Header("Events")]
        [SerializeField] private GameEvent _tryDig = null;
        [SerializeField] private GameEvent _digCompleted = null;
        [SerializeField] private GameEvent _triggerInteraction = null;
        [SerializeField] private GameEvent _chooseNextUsableItem = null;
        [SerializeField] private GameEvent _choosePreviousUsableItem = null;
        [SerializeField] private GameEvent _useItemRequest = null;
        [SerializeField] private GameEvent _createWindow = null;
        [SerializeField] private GameEvent _playerTranslated = null;

        private bool _locked = true;
        private SpriteRenderer _sprite = null;
        private ParticleSystem.EmissionModule _rocketEmission;
        private Animator _animator = null;
        private PlayerRaycaster _raycaster = null;
        private PlayerStatusController _status = null;
        private PlayerAudioController _audioController = null;
        
        private Grid _worldGrid = null;
        private Rigidbody2D _rigidbody = null;
        private bool _isFacingRight = true;
        private float _offsetY_playerCenter_cellCenter = 0f;
        private bool _isNight = false;

        public bool IsFacingRight
        {
            get => _isFacingRight;
            set
            {
                if (_animator.GetBool("Flip") == true) return;

                if (_isFacingRight != value)
                {
                    _animator.SetBool("Flip", true);
                }
                
                _isFacingRight = value;
            }
        }

        public void Flip()
        {
            _sprite.flipX = !_sprite.flipX;
            _animator.SetBool("Flip", false);
        }

        private float CalculateHeightFromGround()
        {
            CircleCollider2D[] tracksColliders = GetComponentsInChildren<CircleCollider2D>();
            CircleCollider2D lowerCollider = tracksColliders.OrderByDescending(x => x.transform.localPosition.y - x.radius).Last();
            BoxCollider2D mainCollider = GetComponent<BoxCollider2D>();
            return lowerCollider.radius + (-lowerCollider.transform.localPosition.y - mainCollider.size.y / 2f) + mainCollider.size.y / 2f;
        }

        #region COROUTINES
        public IEnumerator FollowToDigPlace(Vector2Int coords, float speed, EDigDirection direction)
        {
            _locked = true;
            Vector3 worldCoords = _worldGrid.GetCellCenterWorld((Vector3Int) coords);
            if (direction == EDigDirection.Left || direction == EDigDirection.Right)
                worldCoords += new Vector3(0, _offsetY_playerCenter_cellCenter, 0);

            float lerpCoeff = 0f;
            Vector3 startPosition = transform.position;
            _rigidbody.simulated = false;

            while(lerpCoeff <= 1f)
            {
                lerpCoeff += speed * Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, worldCoords, lerpCoeff);
                yield return null;
            }
            
            _digCompleted.Raise();
            _rigidbody.simulated = true;
            _locked = false;
        }
        #endregion

        #region EVENT RESPONSES

        public void OnWorldLoaded(EventArgs args)
        {
            if (args is WorldLoadedEA wl)
            {
                _worldGrid = wl.WorldGrid;
                OnTranslatePlayer(new TranslatePlayerEA(wl.PlayerSpawnPoint.position));
                _locked = false;
                enabled = true;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnAllowDig(EventArgs args)
        {
            if (args is AllowDigEA ltdp)
            {
                StartCoroutine(FollowToDigPlace(ltdp.Place, ltdp.Speed, ltdp.Direction));
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnMassChanged(int oldMass, int newMass)
        {
            _rigidbody.mass += (newMass - oldMass);
        }

        public void OnTranslatePlayer(EventArgs args)
        {
            if (args is TranslatePlayerEA mp)
            {
                Vector2 oldPosition = transform.position;
                Vector2Int oldGridPosition = _gridPosition.Value;
                transform.position = mp.Position;
                _gridPosition.Value = (Vector2Int)_worldGrid.WorldToCell(transform.position);
                _playerTranslated.Raise(new PlayerTranslatedEA(oldPosition, transform.position, oldGridPosition, _gridPosition.Value));
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void OnConfirmKeyPressed()
        {
            if(!_locked && Time.timeScale > 0f)
                _triggerInteraction.Raise();
        }

        private void OnPreviousKeyPressed()
        {
            _choosePreviousUsableItem.Raise();
        }

        private void OnNextKeyPressed()
        {
            _chooseNextUsableItem.Raise();
        }

        private void OnUseKeyPressed()
        {
            if (!_locked && Time.timeScale > 0f)
                _useItemRequest.Raise();
        }

        private void OnInventoryKeyPressed()
        {
            if (!_locked)
                _createWindow.Raise(new CreateWindowEA(_inventoryWindowPrefab));
        }

        private void OnUpMoveKeyPressed()
        {
            _rocketEmission.enabled = true;
        }

        private void OnUpMoveKeyUp()
        {
            _rocketEmission.enabled = false;
        }

        public void OnDayBegan()
        {
            _light.gameObject.SetActive(false);
            _isNight = false;
        }

        public void OnNightBegan()
        {
            _light.gameObject.SetActive(true);
            _isNight = true;
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                if(pctl.LayerNumber == 0)
                {
                    if (_isNight)
                        _light.gameObject.SetActive(true);
                    else
                        _light.gameObject.SetActive(false);
                }
                else
                    _light.gameObject.SetActive(true);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _raycaster = GetComponent<PlayerRaycaster>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _status = GetComponentInChildren<PlayerStatusController>();
            _audioController = GetComponentInChildren<PlayerAudioController>();
            _rocketEmission = _rocket.emission;
            _playerCargoMass.ValueChanged += OnMassChanged;

            _input.ConfirmKeyPressed += OnConfirmKeyPressed;
            _input.PreviousKeyPressed += OnPreviousKeyPressed;
            _input.NextKeyPressed += OnNextKeyPressed;
            _input.UseKeyPressed += OnUseKeyPressed;
            _input.InventoryViewKeyPressed += OnInventoryKeyPressed;
            _input.UpMoveKeyPressed += OnUpMoveKeyPressed;
            _input.UpMoveKeyUp += OnUpMoveKeyUp;

            _offsetY_playerCenter_cellCenter = -0.5f + CalculateHeightFromGround();
        }

        private void Update()
        {
            _playerPosition.Value = transform.position;
            _currentSpeed.Value = _rigidbody.velocity;
            _gridPosition.Value = (Vector2Int)_worldGrid.WorldToCell(transform.position);
            if (_rigidbody.velocity.magnitude > _maxSpeed)
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
            if (_rigidbody.velocity.x > 0.1f)
                IsFacingRight = true;
            else if(_rigidbody.velocity.x < -0.1f)
                IsFacingRight = false;

            transform.rotation = Quaternion.Euler(0, 0, -_rigidbody.velocity.x);

            if (_locked) return;
            _raycaster.UpdateRaycasts();
            if(_raycaster.IsGrounded == true)
            {
                if(_input.VerticalMove < -0.6f) //dig down
                {
                    _tryDig.Raise(new TryDigEA(_gridPosition + Vector2Int.down, _drillSharpness.Value, 1, transform.position, EDigDirection.Down));
                }
                else if (_raycaster.CanDigRight && _input.HorizontalMove > 0.8f) //dig right
                {
                    _tryDig.Raise(new TryDigEA(_gridPosition + Vector2Int.right, _drillSharpness.Value, 1, transform.position, EDigDirection.Right));
                }
                else if (_raycaster.CanDigLeft && _input.HorizontalMove < -0.8f) //dig left
                {
                    _tryDig.Raise(new TryDigEA(_gridPosition + Vector2Int.left, _drillSharpness.Value, 1, transform.position, EDigDirection.Left));
                }
            }
        }

        private void FixedUpdate()
        {
            if (_locked) return;
            _rigidbody.AddForce(new Vector2(_input.HorizontalMove * _playerEnginePower.Value, Mathf.Clamp(_input.VerticalMove * 2 * _playerEnginePower.Value, 0f, float.MaxValue)));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _rigidbody.simulated = true;
        }

        private void OnDestroy()
        {
            _playerCargoMass.ValueChanged -= OnMassChanged;

            _input.ConfirmKeyPressed -= OnConfirmKeyPressed;
            _input.PreviousKeyPressed -= OnPreviousKeyPressed;
            _input.NextKeyPressed -= OnNextKeyPressed;
            _input.UseKeyPressed -= OnUseKeyPressed;
            _input.InventoryViewKeyPressed -= OnInventoryKeyPressed;
            _input.UpMoveKeyPressed -= OnUpMoveKeyPressed;
            _input.UpMoveKeyUp -= OnUpMoveKeyUp;
        }

        #endregion
    }
}