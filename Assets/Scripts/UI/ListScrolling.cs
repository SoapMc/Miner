using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.UI
{
    public class ListScrolling : MonoBehaviour
    {
        private Coroutine _currentViewMoving = null;

        public void MoveViewRequest(Vector2 target)
        {
            if (_currentViewMoving != null)
                StopCoroutine(_currentViewMoving);
            _currentViewMoving = StartCoroutine(MoveViewToPosition(-target));
        }

        public void MoveViewImmediately(Vector2 target)
        {
            transform.localPosition = target;
        }

        private IEnumerator MoveViewToPosition(Vector2 target)
        {
            float lerpCoeff = 0f;
            while (lerpCoeff < 1f)
            {
                transform.localPosition = Vector2.Lerp(transform.localPosition, target, lerpCoeff);
                lerpCoeff += Time.unscaledDeltaTime;
                yield return null;
            }
            _currentViewMoving = null;
        }
    }
}