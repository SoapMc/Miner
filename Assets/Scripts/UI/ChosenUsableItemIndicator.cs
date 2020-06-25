using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;
using System.Linq;
using Miner.Gameplay;

namespace Miner.UI
{
    public class ChosenUsableItemIndicator : MonoBehaviour
    {
        [SerializeField] private UsableItemList _usableItemList = null;
        [SerializeField] private ChosenUsableItem _chosenUsableItemPrefab = null;
        [SerializeField] private float _spacing = 10f;
        private List<ChosenUsableItem> _content = new List<ChosenUsableItem>();
        private float _itemWidth = 0;
        private Coroutine _currentViewMoving = null;

        public void OnUpdatePlayerData(EventArgs args)
        {
            if(args is UpdatePlayerDataEA upd)
            {
                foreach (var usableItem in upd.AddUsableItemsChange)
                {
                    ChosenUsableItem item = _content.FirstOrDefault(x => x.Item == usableItem.Item);
                    if(item != null)
                    {
                        item.UpdateElement(usableItem.Amount);
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
                        throw new UsableItemNotFoundException();
                    }
                }

                foreach (var usableItem in upd.RemoveUsableItemsChange)
                {
                    ChosenUsableItem item = _content.FirstOrDefault(x => x.Item == usableItem.Item);
                    if (item != null)
                    {
                        item.UpdateElement(-usableItem.Amount);
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
                        throw new UsableItemNotFoundException();
                    }
                }
            }
            else
            {
                throw new InvalidEventArgsException();
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
                throw new InvalidEventArgsException();
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
                lerpCoeff += Time.deltaTime;
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
    }
}