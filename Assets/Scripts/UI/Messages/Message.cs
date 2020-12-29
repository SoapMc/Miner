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
        [SerializeField, Range(0f, 10f)] protected float _delayFromActivation = 0f;
        [System.NonSerialized] protected string _runtimeMessage;

        public void OverrideMessage(string newMessage)
        {
            _runtimeMessage = newMessage;
        }

        public void Show()
        {
            _createMessage.Raise(new CreateMessageEA(_title, _runtimeMessage, _type));
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

        private void OnEnable()
        {
            _runtimeMessage = _message;
        }

        public enum EType
        {
            Tip,
            Statement,
            Warning
        }
    }
}