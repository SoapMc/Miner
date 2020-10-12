using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;

namespace Miner.UI
{
    public class CargoDisplay : MonoBehaviour
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
        }

        public void OnPlayerUnloaded()
        {
            for(int i = 0; i < _elements.Count; ++i)
            {
                _elements[i].UpdateElement(0);
                _elements[i].transform.localPosition = new Vector2(-_elementSize.x, 0);
            }
        }

        public void OnPlayerResourcesGathered(EventArgs args)
        {
            if(args is ResourcesGatheredEA rg)
            {
                float yPosOfAddedElement = 0;
                float yMoveOtherElements = 0f;
                
                foreach (var resource in rg.Resources)
                {
                    int foundIndex = _elements.FindIndex(x => x.TileType == resource.Type);
                    if(_elements[foundIndex].Amount == 0)
                    {
                        for(int i = 0; i < foundIndex; ++i)
                        {
                            if (_elements[i].Amount != 0) yPosOfAddedElement -= _elementSize.y + _spacing.y;   
                        }

                        _elements[foundIndex].transform.localPosition = new Vector2(-_elementSize.x, yPosOfAddedElement);
                        _elements[foundIndex].RequestTranslation(new Vector2(_elementSize.x, 0));
                        yMoveOtherElements -= _elementSize.y + _spacing.y;

                        for (int i = foundIndex + 1; i < _elements.Count; ++i)
                        {
                            if(yMoveOtherElements != 0f && _elements[i].Amount > 0)
                            {
                                _elements[i].RequestTranslation(new Vector2(0, yMoveOtherElements));
                            }
                        }
                    }
                    _elements[foundIndex].UpdateElement(_elements[foundIndex].Amount + resource.Amount);
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnPlayerResourcesRemoved(EventArgs args)
        {
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
                throw new InvalidEventArgsException();
            }
        }
    }
}