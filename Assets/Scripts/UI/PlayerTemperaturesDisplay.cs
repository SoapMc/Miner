using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Miner.UI
{
    public class PlayerTemperaturesDisplay : MonoBehaviour
    {
        [SerializeField] private Image _sliderFill = null;
        [SerializeField] private Slider _heatFlowSlider = null;
        [SerializeField] private FloatReference _playerInternalTemperature = null;
        [SerializeField] private FloatReference _playerExternalTemperature = null;
        [SerializeField] private TextMeshProUGUI _TempText = null;
        [SerializeField] private FloatReference _heatFlow = null;
        [SerializeField] private Color _heating = Color.red;
        [SerializeField] private Color _cooling = Color.blue;
        [SerializeField] private float _heatScale = 1f;
        private int _internalTemp;
        private int _externalTemp;

        private IEnumerator RefreshDisplay()
        {
            while (true)
            {
                _internalTemp = (int)_playerInternalTemperature.Value;
                _externalTemp = (int)_playerExternalTemperature.Value;
                _heatFlowSlider.value = Mathf.Abs(_heatFlow.Value / _heatScale);
                _TempText.text = _internalTemp.ToString() + " / " + _externalTemp.ToString() + " C deg";
                

                if(_heatFlow.Value >= 0f)
                {
                    _sliderFill.color = _heating;
                    _TempText.color = _heating;
                }
                else
                {
                    _sliderFill.color = _cooling;
                    _TempText.color = _cooling;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(RefreshDisplay());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}