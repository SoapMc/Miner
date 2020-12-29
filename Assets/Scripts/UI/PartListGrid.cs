using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using Miner.Management.Events;
using Miner.FX;
using System;

namespace Miner.UI
{
    public class PartListGrid : MonoBehaviour
    {
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _changeEquipment = null;
        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField] private PartGridElement _partGridElementPrefab = null;
        [SerializeField] private ReferencePartList _partList = null;
        [SerializeField] private SoundEffect _buyPart = null;
        [SerializeField] private Vector2 _spacingBetweenElements = Vector2.zero;
        private Coroutine _currentViewMoving = null;
        private List<List<PartGridElement>> _partGridElements = new List<List<PartGridElement>>();

        public void MoveViewRequest(Vector2 target)
        {
            if (_currentViewMoving != null)
                StopCoroutine(_currentViewMoving);
            _currentViewMoving = StartCoroutine(MoveViewToPosition(-target));
        }

        private IEnumerator MoveViewToPosition(Vector2 target)
        {
            float lerpCoeff = 0f;
            while(lerpCoeff < 1f)
            {
                transform.localPosition = Vector2.Lerp(transform.localPosition, target, lerpCoeff);
                lerpCoeff += Time.unscaledDeltaTime;
                yield return null;
            }
            _currentViewMoving = null;
        }

        private List<PartGridElement> LoadParts(List<ReferencePart> parts, int row, Part equippedPart)
        {
            Vector2 elementSize = _partGridElementPrefab.GetComponent<RectTransform>().sizeDelta;
            List<PartGridElement> pgeList = new List<PartGridElement>(parts.Count);

            for (int i = 0; i < parts.Count; ++i)
            {
                PartGridElement pge = Instantiate(_partGridElementPrefab, transform);
                pge.transform.localPosition = new Vector2(i * elementSize.x + i * _spacingBetweenElements.x, -row * elementSize.y - (row + 1) * _spacingBetweenElements.y);
                pge.Initialize(this, parts[i], row);

                if (_playerMoney >= parts[i].Cost)
                {
                    pge.Refresh(EOfferState.Available);
                }
                else
                {
                    pge.Refresh(EOfferState.Unavailable);
                }

                if (equippedPart != null)
                {
                    if (equippedPart.Id == parts[i].Id)
                    {
                        pge.Refresh(EOfferState.Bought);
                    }
                }

                pgeList.Add(pge);
            }

            return pgeList;
        }

        public void BuyPart(PartGridElement element)
        {

            if (element.ReferencePart.Cost <= _playerMoney.Value)
            {
                foreach(var elem in _partGridElements[element.Row])
                {
                    if(elem.CurrentState == EOfferState.Bought)
                    {
                        if (_playerMoney >= elem.ReferencePart.Cost)
                        {
                            elem.Refresh(EOfferState.Available);
                        }
                        else
                        {
                            elem.Refresh(EOfferState.Unavailable);
                        }
                    }
                }

                element.Refresh(EOfferState.Bought);
                _updatePlayerData.Raise(new UpdatePlayerDataEA() { MoneyChange = -element.ReferencePart.Cost });
                _changeEquipment.Raise(new ChangeEquipmentEA() { PartsToEquip = new List<Part>() { element.ReferencePart.CreatePart(1f) } });
                _buyPart.Play();

                for (int row = 0; row < _partGridElements.Count; ++row)
                {
                    foreach (var elem in _partGridElements[row])
                    {
                        if (elem.CurrentState == EOfferState.Bought) continue;

                        if (_playerMoney >= elem.ReferencePart.Cost)
                        {
                            elem.Refresh(EOfferState.Available);
                        }
                        else
                        {
                            elem.Refresh(EOfferState.Unavailable);
                        }
                    }
                }
                
            }

        }

        public void Load()
        {
            int row = 0;
            foreach (EPartType partType in Enum.GetValues(typeof(EPartType)))
            {
                _partGridElements.Add(LoadParts(_partList.GetPartsOfType(partType), row, _playerEquipment.GetEquippedPart(partType)));
                row++;
            }
        }

        public Selectable GetFirstSelectedObject()
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).GetComponent<Selectable>();   
            }
            return null;
        }
    }
}