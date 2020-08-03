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
    public class ProcessingPlantElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _amount = null;
        [SerializeField] private TextMeshProUGUI _price = null;
        [Header("Events")]
        [SerializeField] private GameEvent _tryChangeResourcesInPlayerCargo = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        private CargoTable.Element _cargoElement = null;

        public void Initialize(CargoTable.Element element)
        {
            _cargoElement = element;
            RefreshText();
        }

        public void SellResource()
        {
            UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
            upd.MoneyChange = _cargoElement.Type.Value;
            _tryChangeResourcesInPlayerCargo.Raise(new TryChangeResourcesInPlayerCargoEA() { ResourcesToRemove = new List<CargoTable.Element>() { new CargoTable.Element() { Type = _cargoElement.Type, Amount = 1 } } });
            _updatePlayerData.Raise(upd);
            if (_cargoElement.Amount == 0)
            {
                SelectNext();
                Destroy(gameObject);
            }
            else
            {
                RefreshText();
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
                throw new InvalidSettingException("Selection of next UI element is requested but there is no selectable object in scope!");

        }
    }
}