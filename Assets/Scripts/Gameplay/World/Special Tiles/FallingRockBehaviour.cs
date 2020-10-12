using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class FallingRockBehaviour : MonoBehaviour
    {
        [SerializeField] private FallingRockParticle _fallingRockParticlePrefab = null;
        [SerializeField] private TileType _overriddenTileType = null;
        [SerializeField] private GameEvent _overrideTiles = null;
        [SerializeField] private LayerMask _playerLayer = default;
        [SerializeField, Range(0f, 1f)] private float _probabilityOfFalling = 0.04f;
        private Coroutine _coroutine = null;
        private float _interval = 1f;
        private float _timeToDeactivation;

        private IEnumerator Raycast()
        {
            _timeToDeactivation = 60f;
            while(true)
            {
                if (Random.Range(0f, 1f) < _probabilityOfFalling)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, _playerLayer);
#if UNITY_EDITOR
                    Debug.DrawRay(transform.position, Vector3.down * 5f, Color.red);
#endif
                    if (hit.collider != null)
                    {
                        if (hit.collider.GetComponent<PlayerController>() != null)
                        {
                            Grid grid = GetComponentInParent<Grid>();
                            Vector3Int gridPos = grid.WorldToCell(transform.position);
                            Destroy(gameObject);
                            _overrideTiles.Raise(new OverrideTilesEA(
                                new Dictionary<Vector2Int, TileType>()
                                {
                                {(Vector2Int)gridPos, _overriddenTileType}
                                }));
                            FallingRockParticle frp = Instantiate(_fallingRockParticlePrefab, transform.position, Quaternion.identity);
                            frp.Initialize((int)Mathf.Abs(gridPos.y / 20f));
                        }
                    }

                    _timeToDeactivation -= _interval;
                    if (_timeToDeactivation <= 0f)
                    {
                        enabled = false;
                        break;
                    }
                }
                yield return new WaitForSeconds(_interval);
            }
        }

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(Raycast());
        }

        private void OnDisable()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject != null)
            {
                enabled = true;
            }
        }
    }
}