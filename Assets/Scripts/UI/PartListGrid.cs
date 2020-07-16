using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using Miner.Management.Events;

namespace Miner.UI
{
    public class PartListGrid : MonoBehaviour
    {
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _showPartDescription = null;

        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField] private PartGridElement _partGridElementPrefab = null;
        [SerializeField] private ReferencePartList _partList = null;
        [SerializeField] private Vector2 _spacingBetweenElements = Vector2.zero;
        private Coroutine _currentViewMoving = null;
        private List<List<PartGridElement>> _partGridElements = new List<List<PartGridElement>>();
        private bool _initialized = false;

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
                lerpCoeff += Time.deltaTime;
                yield return null;
            }
            _currentViewMoving = null;
        }

        private List<PartGridElement> LoadParts(List<ReferencePart> parts, int row, ReferencePart equippedPart)
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
                    pge.Refresh(PartGridElement.State.Available);
                }
                else
                {
                    pge.Refresh(PartGridElement.State.Unavailable);
                }

                if (equippedPart != null)
                {
                    if (equippedPart.Id == parts[i].Id)
                    {
                        pge.Refresh(PartGridElement.State.Bought);
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
                element.Refresh(PartGridElement.State.Bought);
                UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
                upd.EquipmentChange.Add(element.ReferencePart);
                upd.MoneyChange = -element.ReferencePart.Cost;
                _updatePlayerData.Raise(upd);

                
                foreach (var elem in _partGridElements[element.Row])
                {
                    if (elem == element) continue;

                    if (_playerMoney >= elem.ReferencePart.Cost)
                    {
                        elem.Refresh(PartGridElement.State.Available);
                    }
                    else
                    {
                        elem.Refresh(PartGridElement.State.Unavailable);
                    }
                }
                
            }

        }

        public void ShowDescription(PartGridElement element)
        {
            _showPartDescription.Raise(new ShowPartDescriptionEA(element.ReferencePart, element.CurrentState));
        }

        private void Awake()
        {

            _partGridElements.Add(LoadParts(_partList.Hulls.OfType<ReferencePart>().ToList(), 0, _playerEquipment.Hull));
            _partGridElements.Add(LoadParts(_partList.FuelTanks.OfType<ReferencePart>().ToList(), 1, _playerEquipment.FuelTank));
            _partGridElements.Add(LoadParts(_partList.Engines.OfType<ReferencePart>().ToList(), 2, _playerEquipment.Engine));
            _partGridElements.Add(LoadParts(_partList.Drills.OfType<ReferencePart>().ToList(), 3, _playerEquipment.Drill));
            _partGridElements.Add(LoadParts(_partList.Coolings.OfType<ReferencePart>().ToList(), 4, _playerEquipment.Cooling));
            _partGridElements.Add(LoadParts(_partList.Cargos.OfType<ReferencePart>().ToList(), 5, _playerEquipment.Cargo));
            _partGridElements.Add(LoadParts(_partList.Batteries.OfType<ReferencePart>().ToList(), 6, _playerEquipment.Battery));

            if (_initialized) return;
            if (transform.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (transform.GetChild(0).gameObject.TryGetComponent(out Selectable s))
                {
                    EventSystem.current.SetSelectedGameObject(s.gameObject);
                    s.OnSelect(null);
                    _initialized = true;
                }
                else
                {
                    Debug.LogWarning("Children have to have Selectable component");
                }
            }
            else
            {
                Debug.LogWarning("The element has to have at least one child!");
            }
        }
    }
}