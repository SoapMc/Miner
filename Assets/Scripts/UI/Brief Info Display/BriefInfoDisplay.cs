using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.FX;
using Miner.Management;

namespace Miner.UI
{
    public class BriefInfoDisplay : MonoBehaviour, IResettableHUDComponent
    {
        [SerializeField] private SoundEffect _textAppearingSound = null;
        [SerializeField] private BriefInfoElement _briefInfoElementPrefab = null;
        [SerializeField] private Color _infoColor = Color.white;
        [SerializeField] private Color _warningColor = Color.white;
        private AudioSource _audioSource = null;
        private List<BriefInfoElement> _elements = new List<BriefInfoElement>();
        private Vector2 _elementSize = Vector2.zero;

        public void OnShowBriefInfo(EventArgs args)
        {
            if(args is ShowBriefInfoEA sbi)
            {
                string[] messages = sbi.Message.Split('\n');

                foreach(var elem in _elements)
                {
                    elem.RequestTranslation(new Vector2(0, _elementSize.y * messages.Length));
                }

                for (int i = 0; i < messages.Length; ++i)
                {
                    BriefInfoElement bie = Instantiate(_briefInfoElementPrefab, transform);
                    bie.Initialize(messages[i], GetColorOfMessageType(sbi.Type));
                    _elements.Add(bie);
                    bie.DisappearingFinished += OnDisappearingFinished;
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        private void OnDisappearingFinished(BriefInfoElement bie)
        {
            bie.DisappearingFinished -= OnDisappearingFinished;
            _elements.Remove(bie);
            Destroy(bie.gameObject);            
        }

        public void ResetComponent()
        {
            for (int i = _elements.Count - 1; i >= 0; i--)
            {
                _elements[i].DisappearingFinished -= OnDisappearingFinished;
                Destroy(_elements[i].gameObject);
            }
            _elements.Clear();
        }

        private Color GetColorOfMessageType(ShowBriefInfoEA.EType type)
        {
            switch(type)
            {
                case ShowBriefInfoEA.EType.Info:
                    return _infoColor;
                case ShowBriefInfoEA.EType.Warning:
                    return _warningColor;
                default:
                    return _infoColor;
            }
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _elementSize = _briefInfoElementPrefab.GetComponent<RectTransform>().sizeDelta;
        }
    }
}