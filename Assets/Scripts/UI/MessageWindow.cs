using Miner.FX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using TMPro;
using UnityEngine.UI;
using System.Text;

namespace Miner.UI
{
    public class MessageWindow : Window
    {
        [SerializeField] private TextMeshProUGUI _title = null;
        [SerializeField] private TextAppearingEffect _message = null;
        [SerializeField] private UIStandardStyleSheet _statementStyle = null;
        [SerializeField] private UIStandardStyleSheet _warningStyle = null;
        [SerializeField] private UIStandardStyleSheet _tipStyle = null;
        private Coroutine _countdown = null;

        public Message.EType MessageType { get; private set; }
        public float DisplayTime { get; private set; }

        protected override void OnAppearingFinished()
        {

        }

        protected override void OnDisappearingFinished()
        {
            Destroy(gameObject);
        }

        protected override void CloseWindow()
        {
            _appearingEffect.TriggerDisappearing();
        }

        public void Close()
        {
            CloseWindow();
        }

        protected override void Awake()
        {
            _appearingEffect = GetComponent<IAppearingEffect>();
            _appearingEffect.AppearingFinished += OnAppearingFinished;
            _appearingEffect.DisappearingFinished += OnDisappearingFinished;
        }

        public void Initialize(string title, string message, Message.EType messageType, float displayTime)
        {
            _title.text = title;
            _message.Show(message);
            MessageType = messageType;
            DisplayTime = displayTime;
            AdjustToType(MessageType);
            _countdown = StartCoroutine(Countdown());
        }

        private void AdjustToType(Message.EType type)
        {
            switch(type)
            {
                case Message.EType.Statement:
                    StyleSheet = _statementStyle;
                    break;
                case Message.EType.Tip:
                    StyleSheet = _tipStyle;
                    break;
                case Message.EType.Warning:
                    StyleSheet = _warningStyle;
                    break;
                default:
                    break;
            }
        }

        private IEnumerator Countdown()
        {
            yield return new WaitForSecondsRealtime(DisplayTime);
            _appearingEffect.TriggerDisappearing();
        }

        protected override void OnDestroy()
        {
            if(_countdown != null)
            {
                StopCoroutine(_countdown);
                _countdown = null;
            }
            base.OnDestroy();
        }
    }
}