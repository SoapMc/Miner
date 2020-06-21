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

        [Header("Events")]
        [SerializeField] private GameEvent _digRequest = null;
        [SerializeField] private GameEvent _digComplete = null;
        [SerializeField] private GameEvent _triggerInteraction = null;

        private Tilemap _groundTilemap = null;
        private Rigidbody2D _rigidbody = null;
        private float _verticalMove = 0f;
        private float _horizontalMove = 0f;
        private float _previousHorizontalMove = 0f;
        private bool _locked = true;

        public int TotalScore { get; private set; }

        private void Flip()
        {
            //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        public void OnWorldLoaded(EventArgs args)
        {
            if (args is WorldLoadedEA wl)
            {
                _groundTilemap = wl.Tilemap;
                transform.position = wl.PlayerSpawnPoint.position;
                _locked = false;
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

        public IEnumerator FollowToDigPlace(Vector2Int coords, float speed)
        {
            _locked = true;
            Vector3 worldCoords = _groundTilemap.GetCellCenterWorld((Vector3Int) coords);
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
        }

        private void Update()
        {
            _playerFuel.Value -= _playerFuelUsage * Time.deltaTime;
            _currentSpeed.Value = _rigidbody.velocity;
            _gridPosition.Value = (Vector2Int)_groundTilemap.WorldToCell(transform.position);

            if (_locked) return;
            _horizontalMove = Input.GetAxis("Horizontal") * _playerEnginePower.Value;
            _verticalMove = Mathf.Clamp(Input.GetAxis("Vertical") * 2 * _playerEnginePower.Value, 0f, float.MaxValue);

            if(_canDigDown)
            {
                if(Input.GetAxis("Vertical") < -0.6f)
                {
                    //dig down
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.down));
                }
                else if (_canDigRight && _horizontalMove > 0.8f)
                {
                    //dig right
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.right));
                }
                else if (_canDigLeft && _horizontalMove < -0.8f)
                {
                    //dig left
                    _digRequest.Raise(new DigRequestEA(_gridPosition + Vector2Int.left));
                }
            }
            
            
            if (_rigidbody.velocity.x * _previousHorizontalMove < 0f)
                Flip();

            _previousHorizontalMove = _rigidbody.velocity.x;

            if(Input.GetKeyDown(KeyCode.Return))
            {
                _triggerInteraction.Raise();
            }
        }

        private void FixedUpdate()
        {
            if (_locked) return;
            _rigidbody.AddForce(new Vector2(_horizontalMove, _verticalMove));
            _horizontalMove = 0f;
            _verticalMove = 0f;
        }
    }
}