using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class PlayerTemperaturesDisplay : MonoBehaviour
    {
        [SerializeField] private FloatReference _playerInternalTemperature = null;
        [SerializeField] private FloatReference _playerExternalTemperature = null;
        [SerializeField] private TextMeshProUGUI _internalTempText = null;
        [SerializeField] private TextMeshProUGUI _externalTempText = null;
        private int _internalTemp;
        private int _externalTemp;

        private IEnumerator RefreshDisplay()
        {
            while (true)
            {
                if (_internalTemp != (int)_playerInternalTemperature.Value)
                {
                    _internalTempText.text = _internalTemp.ToString() + " C deg";
                }
                if (_externalTemp != (int)_playerExternalTemperature.Value)
                {
                    _externalTempText.text = _externalTemp.ToString() + " C deg";
                }
                _internalTemp = (int)_playerInternalTemperature.Value;
                _externalTemp = (int)_playerExternalTemperature.Value;
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void Awake()
        {
            StartCoroutine(RefreshDisplay());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}