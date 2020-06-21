using UnityEngine.EventSystems;
using UnityEngine;
using Miner.Gameplay;
using Miner.Management.Events;
using UnityEngine.UI;

namespace Miner.UI
{
    public class ProcessingPlantWindow : MonoBehaviour
    {
        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private CargoTable _playerCargo = null;
        [SerializeField] private ProcessingPlantElement _processingPlantElementPrefab = null;
        [SerializeField] private Transform _layout = null;
        [SerializeField] private Selectable _firstSelectedObject = null;

        [Header("Events")]
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _closeWindow = null;

        public void CloseWindow()
        {
            _closeWindow.Raise(new CloseWindowEA(gameObject));
        }

        public void SellAll()
        {
            int sum = 0;
            UpdatePlayerDataEA upd = new UpdatePlayerDataEA();

            foreach (var element in _playerCargo)
            {
                sum += element.Type.Value * element.Amount;
                upd.RemoveCargoChange.Add(element);
            }
            upd.MoneyChange = sum;
            _updatePlayerData.Raise(upd);

            foreach(Transform child in _layout)
            {
                Destroy(child.gameObject);
            }
        }

        private void Awake()
        {
            Time.timeScale = 0f;
            foreach(var element in _playerCargo)
            {
                ProcessingPlantElement ppe = Instantiate(_processingPlantElementPrefab, _layout);
                ppe.Initialize(element);
            }

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_firstSelectedObject.gameObject);
            _firstSelectedObject.OnSelect(null);

            if (_playerCargo.Count > 0)
            {
                _firstSelectedObject.navigation = new Navigation() { mode = Navigation.Mode.Explicit, selectOnDown = _layout.GetChild(0).GetComponent<Selectable>(), selectOnUp = _layout.GetChild(_layout.childCount - 1).GetComponent<Selectable>() };
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseWindow();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}