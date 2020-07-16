using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    [RequireComponent(typeof(PlayerInput), typeof(PlayerRaycaster), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private IntReference _playerEnginePower = null;
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerFuelUsage = null;
        [SerializeField] private Vector2IntReference _gridPosition = null;
        [SerializeField] private Vector2Reference _currentSpeed = null;
        [SerializeField] private FloatReference _drillSharpness = null;
        [SerializeField] private IntReference _playerCargoMass = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private Transform _playerCanvas = null;
        [SerializeField] private ParticleSystem _rocket = null;

        [Header("Events")]
        [SerializeField] private GameEvent _digRequest = null;
        [SerializeField] private GameEvent _digComplete = null;
        [SerializeField] private GameEvent _triggerInteraction = null;
        [SerializeField] private GameEvent _chooseNextUsableItem = null;
        [SerializeField] private GameEvent _choosePreviousUsableItem = null;
        [SerializeField] private GameEvent _useItemRequest = null;
        [SerializeField] private GameEvent _playerDead = null;

        private SpriteRenderer _sprite = null;
        private ParticleSystem.EmissionModule _rocketEmission;
        private Animator _animator = null;
        private PlayerInputSheet _input = null;
        private PlayerRaycaster _raycaster = null;
        private float _maxSpeed = 25f;
        private Grid _worldGrid = null;
        private Rigidbody2D _rigidbody = null;
        private bool _isFacingRight = true;
        private bool _locked = true;

        public bool IsFacingRight
        {
            get => _isFacingRight;
            set
            {
                if (_isFacingRight != value)
                {
                    _animator.SetTrigger("Flip");
                }
                _isFacingRight = value;
            }
        }

        public void Flip()
        {
            _sprite.flipX = !_sprite.flipX;
        }

        #region COROUTINES
        public IEnumerator FollowToDigPlace(Vector2Int coords, float speed)
        {
            _locked = true;
            Vector3 worldCoords = _worldGrid.GetCellCenterWorld((Vector3Int) coords);
            float lerpCoeff = 0f;
            Vector3 startPosition = transform.position;
            _rigidbody.simulated = false;
            while(lerpCoeff <= 1f)
            {
                lerpCoeff += speed * Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, worldCoords, lerpCoeff);
                yield return null;
            }
            
            _digComplete.Raise();
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
                transform.position = wl.PlayerSpawnPoint.position;
                _locked = false;
                enabled = true;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnLeadToDigPlace(EventArgs args)
        {
            if (args is LeadToDigPlaceEA ltdp)
            {
                StartCoroutine(FollowToDigPlace(ltdp.Place, ltdp.Speed));
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

        public void OnRestoreGameAfterPlayerDestroyed(EventArgs args)
        {
            if (args is RestoreGameAfterPlayerDestroyedEA rgapd)
            {
                transform.position = rgapd.PlayerSpawnPoint.position;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnMovePlayer(EventArgs args)
        {
            if (args is MovePlayerEA mp)
            {
                transform.position = mp.Position;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void OnConfirmKeyPressed()
        {
            _triggerInteraction.Raise();
        }

        public void OnCancelKeyPressed()
        {

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
            _useItemRequest.Raise(new UseItemRequestEA(_gridPosition));
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _raycaster = GetComponent<PlayerRaycaster>();
            _input = GetComponent<PlayerInput>().InputSheet;
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _rocketEmission = _rocket.emission;
            _playerCargoMass.ValueChanged += OnMassChanged;

            _input.ConfirmKeyPressed += OnConfirmKeyPressed;
            _input.CancelKeyPressed += OnCancelKeyPressed;
            _input.PreviousKeyPressed += OnPreviousKeyPressed;
            _input.NextKeyPressed += OnNextKeyPressed;
            _input.UseKeyPressed += OnUseKeyPressed;
        }

        private void Update()
        {
            _playerPosition.Value = transform.position;
            _playerFuel.Value -= _playerFuelUsage * Time.deltaTime;
            if(_playerFuel.Value <= 0f)
                _playerDead.Raise();
            _currentSpeed.Value = _rigidbody.velocity;
            _gridPosition.Value = (Vector2Int)_worldGrid.WorldToCell(transform.position);
            if (_rigidbody.velocity.magnitude > _maxSpeed)
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
            if (_rigidbody.velocity.x > 0.1f)
                IsFacingRight = true;
            else if(_rigidbody.velocity.x < -0.1f)
                IsFacingRight = false;

            if (_input.VerticalMove > 0.1f)
                _rocketEmission.enabled = true;
            else
                _rocketEmission.enabled = false;


            //transform.rotation = Quaternion.Euler(0, 0, -_rigidbody.velocity.x);
            //_playerCanvas.rotation = Quaternion.Euler(0, 0, _rigidbody.velocity.x);

            if (_locked) return;

            if(_raycaster.CanDigDown == true)   //is grounded
            {
                if(_input.VerticalMove < -0.6f) //dig down
                {
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.down, _drillSharpness.Value, transform.position));
                }
                else if (_raycaster.CanDigRight && _input.HorizontalMove > 0.8f) //dig right
                {
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.right, _drillSharpness.Value, transform.position));
                }
                else if (_raycaster.CanDigLeft && _input.HorizontalMove < -0.8f) //dig left
                {
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.left, _drillSharpness.Value, transform.position));
                }
            }
        }

        private void FixedUpdate()
        {
            if (_locked) return;
            _rigidbody.AddForce(new Vector2(_input.HorizontalMove * _playerEnginePower.Value, Mathf.Clamp(_input.VerticalMove * 2 * _playerEnginePower.Value, 0f, float.MaxValue)));
        }

        private void OnDestroy()
        {
            _playerCargoMass.ValueChanged -= OnMassChanged;

            _input.ConfirmKeyPressed -= OnConfirmKeyPressed;
            _input.CancelKeyPressed -= OnCancelKeyPressed;
            _input.PreviousKeyPressed -= OnPreviousKeyPressed;
            _input.NextKeyPressed -= OnNextKeyPressed;
            _input.UseKeyPressed -= OnUseKeyPressed;
        }

        #endregion
    }
}