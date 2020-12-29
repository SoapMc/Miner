using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;
using System.Linq;
using Miner.Gameplay;
using UsableItemsChangedEA = Miner.Management.Events.ChangeUsableItemsEA;
using Miner.Management;

namespace Miner.UI
{
    public class ChosenUsableItemIndicator : MonoBehaviour, IResettableHUDComponent
    {
        [SerializeField] private UsableItemList _usableItemList = null;
        [SerializeField] private ChosenUsableItem _chosenUsableItemPrefab = null;
        [SerializeField] private float _spacing = 0f;
        private List<ChosenUsableItem> _content = new List<ChosenUsableItem>();
        private float _itemWidth = 0f;
        private Coroutine _currentViewMoving = null;

        public void OnUsableItemsChanged(EventArgs args)
        {
            if(args is UsableItemsChangedEA cui)
            {
                foreach (var usableItem in cui.AddedUsableItems)
                {
                    ChosenUsableItem item = _content.FirstOrDefault(x => x.Item == usableItem.Item);
                    if (item != null)
                    {
                        item.UpdateElement(item.Amount + usableItem.Amount);
                        if (item.Amount > 0)
                        {
                            item.gameObject.SetActive(true);
                            UpdateLayout();
                            if (_content.FindAll(x => x.gameObject.activeSelf == true).Count == 1)
                                MoveViewRequest(item.transform.localPosition);
                        }
                    }
                    else
                    {
                        Log.Instance.WriteException(new UsableItemNotFoundException());
                    }
                }

                foreach (var usableItem in cui.RemovedUsableItems)
                {
                    ChosenUsableItem item = _content.FirstOrDefault(x => x.Item == usableItem.Item);
                    if (item != null)
                    {
                        item.UpdateElement(item.Amount - usableItem.Amount);
                        if (item.Amount <= 0)
                        {
                            item.gameObject.SetActive(false);
                            UpdateLayout();
                            if (_content.FindAll(x => x.gameObject.activeSelf == true).Count > 0)
                                MoveViewRequest(_content.First(x => x.gameObject.activeSelf == true).transform.localPosition);
                        }
                    }
                    else
                    {
                        Log.Instance.WriteException(new UsableItemNotFoundException());
                    }
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnChooseUsableItem(EventArgs args)
        {
            if(args is ChooseUsableItemEA cui)
            {
                ChosenUsableItem item = _content.FirstOrDefault(x => x.Item == cui.Item);
                if(item != null)
                {
                    if(item.gameObject.activeSelf == true)
                        MoveViewRequest(item.transform.localPosition);
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }
        public void MoveViewRequest(Vector2 target)
        {
            if (_currentViewMoving != null)
                StopCoroutine(_currentViewMoving);
            _currentViewMoving = StartCoroutine(MoveViewToPosition(-target));
        }

        private IEnumerator MoveViewToPosition(Vector2 target)
        {
            float lerpCoeff = 0f;
            while (lerpCoeff < 0.999f)
            {
                transform.localPosition = Vector2.Lerp(transform.localPosition, target, lerpCoeff);
                lerpCoeff += Time.unscaledDeltaTime;
                yield return null;
            }
            _currentViewMoving = null;
        }


        private void UpdateLayout()
        {
            float xPosition = 0;
            foreach(var item in _content)
            {
                if(item.gameObject.activeSelf == true)
                {
                    item.transform.localPosition = new Vector3(xPosition, 0, 0);
                    xPosition += _itemWidth + _spacing;
                }
            }
        }

        private void Awake()
        {
            _itemWidth = _chosenUsableItemPrefab.GetComponent<RectTransform>().sizeDelta.x;

            foreach(var usableItem in _usableItemList)
            {
                ChosenUsableItem cui = Instantiate(_chosenUsableItemPrefab, transform);
                cui.gameObject.SetActive(false);
                cui.Initialize(usableItem, 0);
                _content.Add(cui);
            }
            UpdateLayout();
        }

        public void ResetComponent()
        {
            if(_currentViewMoving != null)
            {
                StopCoroutine(_currentViewMoving);
                _currentViewMoving = null;
            }

            transform.localPosition = Vector3.zero;

            for (int i = _content.Count - 1; i >= 0; i--)
            {
                _content[i].UpdateElement(0);
                _content[i].gameObject.SetActive(false);
            }
            UpdateLayout();
        }
    }
}