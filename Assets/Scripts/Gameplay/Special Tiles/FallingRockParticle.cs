using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class FallingRockParticle : MonoBehaviour
    {
        [SerializeField] private GameEvent _playerDamaged = null;
        [SerializeField] private DamageType _damageType = null;
        private float _lifeTime = 5f;
        private float _elapsedTime = 0f;
        private int _damage;

        public void Initialize(int damage)
        {
            _damage = damage;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _lifeTime)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject != null)
            {
                if(collision.gameObject.GetComponent<PlayerController>())
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    int damage = Mathf.Clamp(_damage * (int) Mathf.Clamp(Mathf.Sqrt(rb.velocity.sqrMagnitude / 10f), 1f, 5f), 1, int.MaxValue);
                    _playerDamaged.Raise(new PlayerDamagedEA(damage, _damageType));
                    Destroy(gameObject);
                }
            }
        }
    }
}