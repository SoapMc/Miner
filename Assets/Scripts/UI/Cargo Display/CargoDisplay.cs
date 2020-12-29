using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;
using Miner.Management;

namespace Miner.UI
{
    public class CargoDisplay : MonoBehaviour, IResettableHUDComponent
    {
        [SerializeField] private CargoDisplayElement _cargoElementPrefab = null;
        [SerializeField] private TileTypes _tileTypes = null;
        [SerializeField] private Vector2 _spacing = new Vector2(0, 10);
        private List<CargoDisplayElement> _elements; 
        private Vector2 _elementSize;

        private void Awake()
        {
            _elementSize = _cargoElementPrefab.GetComponent<RectTransform>().sizeDelta;
            
            _elements = new List<CargoDisplayElement>(_tileTypes.Count);
            foreach (var tile in _tileTypes)
            {
                if(tile.IsCollectible && !tile.IsFuel)
                {
                    CargoDisplayElement cde = Instantiate(_cargoElementPrefab, gameObject.transform);
                    cde.Initialize(tile);
                    _elements.Add(cde);
                    cde.transform.localPosition = new Vector2(-_elementSize.x, 0);
                }
            }
            _elements = _elements.OrderBy(x => x.TileType.Id).ToList();
        }

        public void OnPlayerResourcesGathered(EventArgs args)
        {
            if (gameObject.activeSelf == false) return;

            if(args is ResourcesGatheredEA rg)
            {
                
                float yTranslateOtherElements = 0f;
                List<CargoTable.Element> addedResources = rg.Resources.OrderBy(x => x.Type.Id).ToList();

                foreach (var resource in addedResources)
                {
                    float yPosOfAddedElement = 0;
                    try
                    {
                        int foundIndex = _elements.FindIndex(x => x.TileType.Id == resource.Type.Id);   //index of tile in collection with found resource
                        if (_elements[foundIndex].Amount == 0)
                        {
                            for (int i = 0; i < foundIndex; ++i)
                            {
                                if (_elements[i].Amount != 0) yPosOfAddedElement -= _elementSize.y + _spacing.y;
                            }

                            _elements[foundIndex].transform.localPosition = new Vector2(-_elementSize.x, yPosOfAddedElement);
                            _elements[foundIndex].RequestTranslation(new Vector2(_elementSize.x, 0));
                            yTranslateOtherElements -= _elementSize.y + _spacing.y;

                            for (int i = foundIndex + 1; i < _elements.Count; ++i)
                            {
                                if (yTranslateOtherElements != 0f && _elements[i].Amount > 0)
                                {
                                    _elements[i].RequestTranslation(new Vector2(0, yTranslateOtherElements));
                                }
                            }
                        }
                        _elements[foundIndex].UpdateElement(_elements[foundIndex].Amount + resource.Amount);
                    }
                    catch(ArgumentNullException ex)
                    {
                        Log.Instance.WriteException(ex);
                        continue;
                    }
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnPlayerResourcesRemoved(EventArgs args)
        {
            if (gameObject.activeSelf == false) return;

            if (args is ResourcesRemovedEA rg)
            {
                float yMoveOtherElements = 0f;
                foreach (var resource in rg.Resources)
                {
                    int foundIndex = _elements.FindIndex(x => x.TileType == resource.Type);
                    _elements[foundIndex].UpdateElement(_elements[foundIndex].Amount - resource.Amount);
                    if(_elements[foundIndex].Amount == 0)
                    {
                        _elements[foundIndex].RequestTranslation(new Vector2(-_elementSize.x, 0));
                        yMoveOtherElements += _elementSize.y + _spacing.y;
                    }

                    for (int i = foundIndex + 1; i < _elements.Count; ++i)
                    {
                        if (_elements[i].Amount > 0)
                        {
                            _elements[i].RequestTranslation(new Vector2(0, yMoveOtherElements));
                        }
                    }
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void ResetComponent()
        {
            for (int i = 0; i < _elements.Count; ++i)
            {
                _elements[i].ResetComponent();
                _elements[i].transform.localPosition = new Vector2(-_elementSize.x, 0);
            }
        }
    }
}