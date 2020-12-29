using UnityEngine.EventSystems;
using UnityEngine;
using Miner.Gameplay;
using Miner.Management.Events;
using UnityEngine.UI;
using Miner.FX;
using System.Collections.Generic;

namespace Miner.UI
{
    public class ProcessingPlantWindow : Window
    {

        [SerializeField] private CargoTable _playerCargo = null;
        [SerializeField] private ProcessingPlantElement _processingPlantElementPrefab = null;
        [SerializeField] private ListScrolling _listScrolling = null;
        [SerializeField] private Selectable _sellAllButton = null;

        [Header("Events")]
        [SerializeField] private GameEvent _tryChangeResourcesInPlayerCargo = null;
        [SerializeField] private GameEvent _updatePlayerData = null;

        
        private List<ProcessingPlantElement> _elements = new List<ProcessingPlantElement>();

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
            foreach(Transform child in _listScrolling.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void SelectSellAllButton()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_sellAllButton.gameObject);
            _sellAllButton.GetComponent<Selectable>().OnSelect(null);
        }

        private void Start()
        {
            for(int i = 0; i < _playerCargo.Count; ++i)
            {
                ProcessingPlantElement ppe = Instantiate(_processingPlantElementPrefab, _listScrolling.transform);
                ppe.transform.localPosition = new Vector2(0, -ppe.Size.y * i);
                ppe.Initialize(_playerCargo[i], _listScrolling, this);
                _elements.Add(ppe);
            }

            if (_elements.Count > 0)
            {
                if (_elements.Count == 1)
                {
                    Selectable firstSelectable = _elements[0].GetComponent<Selectable>();
                    firstSelectable.navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = null,
                        selectOnLeft = _firstSelectedObject,
                        selectOnRight = _firstSelectedObject,
                        selectOnUp = null
                    };
                }
                else if (_elements.Count == 2)
                {
                    for (int i = 1; i < _elements.Count - 1; ++i)
                    {
                        Selectable s = _elements[i].GetComponent<Selectable>();
                        s.navigation = new Navigation()
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnDown = _elements[i].GetComponent<Selectable>(),
                            selectOnLeft = _firstSelectedObject,
                            selectOnRight = _firstSelectedObject,
                            selectOnUp = _elements[i].GetComponent<Selectable>()
                        };
                    }
                }
                else if (_elements.Count > 2)
                {
                    Selectable firstSelectable = _elements[0].GetComponent<Selectable>();
                    firstSelectable.navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = _elements[1].GetComponent<Selectable>(),
                        selectOnLeft = _firstSelectedObject,
                        selectOnRight = _firstSelectedObject,
                        selectOnUp = _elements[_elements.Count - 1].GetComponent<Selectable>()
                    };

                    for (int i = 1; i < _elements.Count - 1; ++i)
                    {
                        Selectable s = _elements[i].GetComponent<Selectable>();
                        s.navigation = new Navigation()
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnDown = _elements[i + 1].GetComponent<Selectable>(),
                            selectOnLeft = _firstSelectedObject,
                            selectOnRight = _firstSelectedObject,
                            selectOnUp = _elements[i - 1].GetComponent<Selectable>()
                        };
                    }

                    Selectable lastSelectable = _elements[_elements.Count - 1].GetComponent<Selectable>();
                    lastSelectable.navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = _elements[0].GetComponent<Selectable>(),
                        selectOnLeft = _firstSelectedObject,
                        selectOnRight = _firstSelectedObject,
                        selectOnUp = _elements[_elements.Count - 2].GetComponent<Selectable>()
                    };
                }

                //set new first selected object
                _firstSelectedObject = _elements[0].GetComponent<Selectable>();
                _listScrolling.MoveViewImmediately(_firstSelectedObject.transform.localPosition);
            }
            else
            {
                _firstSelectedObject = _sellAllButton;
            }
        }
    }
}