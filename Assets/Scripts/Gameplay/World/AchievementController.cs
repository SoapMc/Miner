using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class AchievementController : MonoBehaviour
    {
        [SerializeField] private IntReference _playerMaxAchievedLayerNumber = null;
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _createMessage = null;
        [Header("Rewards for reaching layer")]
        [SerializeField] private List<int> _rewards = new List<int>();

        private void OnReachNewLayer(int oldVal, int newVal)
        {
            int layerIndex = Mathf.Clamp(newVal, 0, _rewards.Count);
            if (_rewards.Count > 0)
            {
                if (_rewards[layerIndex] > 0)
                {
                    UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
                    upd.MoneyChange += _rewards[layerIndex];
                    _updatePlayerData.Raise(upd);
                    _createMessage.Raise(new CreateMessageEA("New layer reached", "You have reached layer at depth of " + _playerGridPosition.Value.y * Management.GameRules.Instance.RealDimensionOfTile + " m. "
                        + _rewards[layerIndex] + " credits have been acquired.", UI.Message.EType.Statement));
                }
            }
        }

        private void Start()
        {
            _playerMaxAchievedLayerNumber.ValueChanged += OnReachNewLayer;
        }

        private void OnDestroy()
        {
            _playerMaxAchievedLayerNumber.ValueChanged -= OnReachNewLayer;
        }
    }
}