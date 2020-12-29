using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class PlayerMoneyDisplay : MonoBehaviour
    {
        [SerializeField] private IntReference _money = null;
        [SerializeField] private TextMeshProUGUI _moneyChangeText = null;
        [SerializeField] private Color _positivaChangeColor = Color.green;
        [SerializeField] private Color _negativeChangeColor = Color.red;
        [SerializeField, Range(0.0001f, 0.1f)] private float _rateOfDisappearingChangeText = 0.01f;
        private TextMeshProUGUI _text = null;
        private int _currentValue;
        private Coroutine _coroutine = null;

        private void OnValueChanged(int oldValue, int newValue)
        {
            try
            {
                if (gameObject.activeInHierarchy == true)
                {
                    if (_coroutine != null)
                        StopCoroutine(_coroutine);
                    _coroutine = StartCoroutine(ToDesiredValue(newValue));
                    StartCoroutine(ShowChange(newValue - oldValue));
                }
                else
                {
                    _currentValue = newValue;
                    _text.text = _currentValue.ToString() + " $";
                }
            }
            catch
            {
                _currentValue = newValue;
                _text.text = _currentValue.ToString() + " $";
            }
        }

        private IEnumerator ToDesiredValue(int desiredValue)
        {
            float lerpCoeff = 0f;
            while(_currentValue != desiredValue)
            {
                lerpCoeff += Time.unscaledDeltaTime;
                _currentValue = Mathf.RoundToInt(Mathf.Lerp(_currentValue, desiredValue, lerpCoeff));
                _text.text = _currentValue.ToString() + " $";
                yield return null;
            }
            _coroutine = null;
        }

        private IEnumerator ShowChange(int change)
        {
            if (change == 0) yield break;

            if (change > 0)
            {
                _moneyChangeText.color = _positivaChangeColor;
                _moneyChangeText.text = "+ " + change.ToString() + " $";
            }
            else
            {
                _moneyChangeText.color = _negativeChangeColor;
                _moneyChangeText.text = change.ToString() + " $";
            }

            float lerpCoeff = 0f;
            while (_moneyChangeText.color.a > 0.001f)
            {
                lerpCoeff += Time.unscaledDeltaTime * _rateOfDisappearingChangeText;
                _moneyChangeText.color *= new Color(1, 1, 1, Mathf.Lerp(1f, 0f, lerpCoeff));
                yield return null;
            }
            _moneyChangeText.color *= new Color(1, 1, 1, 0);
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _money.ValueChanged += OnValueChanged;
            OnValueChanged(_money.Value, _money.Value);
            _moneyChangeText.color = new Color(0, 0, 0, 0);
        }

        private void OnDestroy()
        {
            _money.ValueChanged -= OnValueChanged;
        }
    }
}