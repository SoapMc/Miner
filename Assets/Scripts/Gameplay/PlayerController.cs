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
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private IntReference _playerEnginePower = null;
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerFuelUsage = null;
        [SerializeField] private Vector2IntReference _gridPosition = null;
        [SerializeField] private BoolReference _canDigRight = null;
        [SerializeField] private BoolReference _canDigDown = null;
        [SerializeField] private BoolReference _canDigLeft = null;
        [SerializeField] private Vector2Reference _currentSpeed = null;
        [SerializeField] private FloatReference _drillSharpness = null;
        [SerializeField] private IntReference _playerCargoMass = null;
        [SerializeField] private Vector2Reference _playerPosition = null;

        [Header("Events")]
        [SerializeField] private GameEvent _digRequest = null;
        [SerializeField] private GameEvent _digComplete = null;
        [SerializeField] private GameEvent _triggerInteraction = null;
        [SerializeField] private GameEvent _chooseNextUsableItem = null;
        [SerializeField] private GameEvent _choosePreviousUsableItem = null;
        [SerializeField] private GameEvent _useItemRequest = null;
        [SerializeField] private GameEvent _playerDead = null;

        private float _maxSpeed = 25f;
        private Grid _worldGrid = null;
        private Rigidbody2D _rigidbody = null;
        private float _verticalMove = 0f;
        private float _horizontalMove = 0f;
        private float _previousHorizontalMove = 0f;
        private bool _locked = true;

        public int TotalScore { get; private set; }

        private void Flip()
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

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
            if(args is LeadToDigPlaceEA ltdp)
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
            if(args is RestoreGameAfterPlayerDestroyedEA rgapd)
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
            if(args is MovePlayerEA mp)
            {
                transform.position = mp.Position;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public IEnumerator FollowToDigPlace(Vector2Int coords, float speed)
        {
            _locked = true;
            Vector3 worldCoords = _worldGrid.GetCellCenterWorld((Vector3Int) coords);
            float lerpCoeff = 0f;
            Vector3 startPosition = transform.position;

            while(Vector2.SqrMagnitude(worldCoords - transform.position) > 0.1f)
            {
                lerpCoeff += speed;
                transform.position = Vector3.Lerp(startPosition, worldCoords, lerpCoeff);
                yield return null;
            }
            _digComplete.Raise();
            _locked = false;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerCargoMass.ValueChanged += OnMassChanged;
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

            if (_locked) return;
            _horizontalMove = Input.GetAxis("Horizontal") * _playerEnginePower.Value;
            _verticalMove = Mathf.Clamp(Input.GetAxis("Vertical") * 2 * _playerEnginePower.Value, 0f, float.MaxValue);

            if(_canDigDown.Value == true)
            {
                if(Input.GetAxis("Vertical") < -0.6f)
                {
                    //dig down
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.down, _drillSharpness.Value, transform));
                }
                else if (_canDigRight && _horizontalMove > 0.8f)
                {
                    //dig right
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.right, _drillSharpness.Value, transform));
                }
                else if (_canDigLeft && _horizontalMove < -0.8f)
                {
                    //dig left
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.left, _drillSharpness.Value, transform));
                }
            }


            if (10E+6 * _rigidbody.velocity.x * _previousHorizontalMove < 0f)
            {
                Debug.Log(_rigidbody.velocity.x + " " + _previousHorizontalMove);
                Flip();
            }

            _previousHorizontalMove = _rigidbody.velocity.x;

            if(Input.GetKeyDown(KeyCode.Return))
            {
                _triggerInteraction.Raise();
            }

            if(Input.GetKeyDown(KeyCode.X))
            {
                _chooseNextUsableItem.Raise();
            }
            else if(Input.GetKeyDown(KeyCode.Z))
            {
                _choosePreviousUsableItem.Raise();
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                _useItemRequest.Raise(new UseItemRequestEA(_gridPosition.Value));
            }
        }

        private void FixedUpdate()
        {
            if (_locked) return;
            _rigidbody.AddForce(new Vector2(_horizontalMove, _verticalMove));
            _horizontalMove = 0f;
            _verticalMove = 0f;
        }

        private void OnDestroy()
        {
            _playerCargoMass.ValueChanged -= OnMassChanged;
        }
    }
}