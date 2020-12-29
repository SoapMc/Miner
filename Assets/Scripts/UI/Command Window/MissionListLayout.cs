using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.FX;

namespace Miner.UI
{
    public class MissionListLayout : MonoBehaviour
    {

        private Coroutine _currentViewMoving = null;

        public void MoveViewRequest(Vector2 target)
        {
            if (_currentViewMoving != null)
                StopCoroutine(_currentViewMoving);
            _currentViewMoving = StartCoroutine(MoveViewToPosition(-target));
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