using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using UnityEngine.UI;
using TMPro;

namespace Miner.UI
{
    [RequireComponent(typeof(Image))]
    public class CargoDisplayElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountText = null;
        private Image _image = null;
        private TileType _tileType = null;
        private int _amount = 0;
        private Coroutine _movingCoroutine = null;

        public TileType TileType => _tileType;
        public int Amount => _amount;

        public void Initialize(TileType tileType, int amount = 0)
        {
            _tileType = tileType;
            _image.sprite = tileType.Icon;
            UpdateElement(amount);
        }

        public void UpdateElement(int amount)
        {
            _amount = amount;
            _amountText.text = _amount.ToString();
        }

        public void RequestTranslation(Vector2 translation)
        {
            if (_movingCoroutine != null)
                StopCoroutine(_movingCoroutine);
            _movingCoroutine = StartCoroutine(MoveToPosition((Vector2)transform.localPosition + translation));
        }

        private IEnumerator MoveToPosition(Vector2 target)
        {
            float lerpCoeff = 0f;
            while (lerpCoeff < 1f)
            {
                transform.localPosition = Vector2.Lerp(transform.localPosition, target, lerpCoeff);
                lerpCoeff += Time.unscaledDeltaTime;
                yield return null;
            }
            _movingCoroutine = null;
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
    }
}