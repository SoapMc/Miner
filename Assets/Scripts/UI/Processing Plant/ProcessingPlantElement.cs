using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;
using Miner.Management.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Miner.Management.Exceptions;

namespace Miner.UI
{
    [RequireComponent(typeof(Selectable))]
    public class ProcessingPlantElement : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _amount = null;
        [SerializeField] private TextMeshProUGUI _price = null;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.white;
        [Header("Events")]
        [SerializeField] private GameEvent _tryChangeResourcesInPlayerCargo = null;
        [SerializeField] private GameEvent _updatePlayerData = null;

        private ProcessingPlantWindow _processingPlantWindow = null;
        private ListScrolling _listScrolling = null;
        private Vector2 _size;
        private CargoTable.Element _cargoElement = null;

        public Vector2 Size => _size;

        public void Initialize(CargoTable.Element element, ListScrolling listScrolling, ProcessingPlantWindow processingPlantWindow)
        {
            _processingPlantWindow = processingPlantWindow;
            _listScrolling = listScrolling;
            _cargoElement = element;
            RefreshText();
            SetColor(_normalColor);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _listScrolling.MoveViewRequest(transform.localPosition);
            SetColor(_selectedColor);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SetColor(_normalColor);
        }

        public void SellResource()
        {
            UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
            upd.MoneyChange = _cargoElement.Type.Value;
            _tryChangeResourcesInPlayerCargo.Raise(new TryChangeResourcesInPlayerCargoEA() { ResourcesToRemove = new List<CargoTable.Element>() { new CargoTable.Element() { Type = _cargoElement.Type, Amount = 1 } } });
            _updatePlayerData.Raise(upd);
            if (_cargoElement.Amount == 0)
            {
                Destroy(gameObject);
                SelectNext();
            }
            else
            {
                RefreshText();
            }
        }

        private void SetColor(Color color)
        {
            Graphic[] graphics = GetComponentsInChildren<Graphic>();
            for(int i = 0; i < graphics.Length; ++i)
            {
                graphics[i].color = color;
            }
        }

        private void RefreshText()
        {
            _name.text = _cargoElement.Type.Name;
            _amount.text = _cargoElement.Amount.ToString() + " x " + _cargoElement.Type.Value.ToString() + " $";
            _price.text = (_cargoElement.Amount * _cargoElement.Type.Value).ToString() + " $";
        }

        private void SelectNext()
        {
            Selectable s = GetComponent<Selectable>().FindSelectableOnDown();
            if (s != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(s.gameObject);
                s.OnSelect(null);
            }
            else
            {
                _processingPlantWindow.SelectSellAllButton();
            }
        }

        private void Awake()
        {
            _size = GetComponent<RectTransform>().sizeDelta;
        }
    }
}