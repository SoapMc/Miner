using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.UI
{
    [CreateAssetMenu(menuName = "Messages/Text Message")]
    public class Message : ScriptableObject
    {
        [SerializeField] protected GameEvent _createMessage = null;
        [SerializeField] protected string _title = string.Empty;
        [SerializeField, TextArea] protected string _message = string.Empty;
        [SerializeField] protected EType _type = default;
        [SerializeField, Range(0f, 10f)] private float _delayFromActivation = 0f;

        public void Show()
        {
            _createMessage.Raise(new CreateMessageEA(_title, _message, _type));
            OnShow();
        }

        public void ShowWithDelay(MonoBehaviour coroutineHolder)
        {
            coroutineHolder.StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            yield return new WaitForSecondsRealtime(_delayFromActivation);
            Show();
        }

        protected virtual void OnShow() { }

        public enum EType
        {
            Tip,
            Statement,
            Warning
        }
    }
}