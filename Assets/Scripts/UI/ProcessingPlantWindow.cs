using UnityEngine.EventSystems;
using UnityEngine;
using Miner.Gameplay;
using Miner.Management.Events;
using UnityEngine.UI;
using Miner.FX;
using System.Collections.Generic;

namespace Miner.UI
{
    public class ProcessingPlantWindow : MonoBehaviour
    {
        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private CargoTable _playerCargo = null;
        [SerializeField] private ProcessingPlantElement _processingPlantElementPrefab = null;
        [SerializeField] private Transform _layout = null;
        [SerializeField] private Selectable _firstSelectedObject = null;
        [SerializeField] private PlayerInputSheet _input = null;

        [Header("Events")]
        [SerializeField] private GameEvent _tryChangeResourcesInPlayerCargo = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _closeWindow = null;

        public void CloseWindow()
        {
            _closeWindow.Raise(new CloseWindowEA(gameObject));
            Time.timeScale = 1f;
            _input.CancelKeyPressed -= CloseWindow;
        }

        public void SellAll()
        {
            int sum = 0;
            UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
            foreach (var element in _playerCargo)
            {
                sum += element.Type.Value * element.Amount;
            }
            upd.MoneyChange = sum;
            _updatePlayerData.Raise(upd);
            _tryChangeResourcesInPlayerCargo.Raise(new TryChangeResourcesInPlayerCargoEA() { ResourcesToRemove = _playerCargo.Get() });
            foreach(Transform child in _layout)
            {
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {
            Time.timeScale = 0f;
            foreach(var element in _playerCargo)
            {
                ProcessingPlantElement ppe = Instantiate(_processingPlantElementPrefab, _layout);
                ppe.Initialize(element);
            }

            EventSystem.current.SetSelectedGameObject(_firstSelectedObject.gameObject);
            _firstSelectedObject.OnSelect(null);

            if (_playerCargo.Count > 0)
            {
                _firstSelectedObject.navigation = new Navigation() { mode = Navigation.Mode.Explicit, selectOnDown = _layout.GetChild(0).GetComponent<Selectable>(), selectOnUp = _layout.GetChild(_layout.childCount - 1).GetComponent<Selectable>() };
            }

            _input.CancelKeyPressed += CloseWindow;
        }
    }
}