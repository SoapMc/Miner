﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Miner.UI
{
    public class PlayerTemperaturesDisplay : MonoBehaviour
    {
        [SerializeField] private Image _fill = null;
        [SerializeField] private FloatReference _playerInternalTemperature = null;
        [SerializeField] private FloatReference _playerExternalTemperature = null;
        [SerializeField] private TextMeshProUGUI _brightText = null;
        [SerializeField] private TextMeshProUGUI _blackText = null;
        [SerializeField] private FloatReference _heatFlow = null;
        [SerializeField] private Color _heating = Color.red;
        [SerializeField] private Color _cooling = Color.blue;
        [SerializeField] private float _heatScale = 1f;

        private Bar _bar = null;
        private int _internalTemp;
        private int _externalTemp;

        private IEnumerator RefreshDisplay()
        {
            while (true)
            {
                _internalTemp = (int)_playerInternalTemperature.Value;
                _externalTemp = (int)_playerExternalTemperature.Value;
                _bar.Value = Mathf.Abs(_heatFlow.Value / _heatScale);
                _brightText.text = _internalTemp.ToString() + " / " + _externalTemp.ToString() + " deg";
                _blackText.text = _brightText.text;

                if(_heatFlow.Value >= 0f)
                {
                    _fill.color = _heating;
                    _brightText.color = _heating;
                }
                else
                {
                    _fill.color = _cooling;
                    _brightText.color = _cooling;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void Awake()
        {
            _bar = GetComponent<Bar>();
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