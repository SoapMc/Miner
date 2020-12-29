using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System.Linq;
using Miner.Management.Exceptions;
using System;
using Random = UnityEngine.Random;

namespace Miner.Gameplay
{
    public class BoxBehaviour : MonoBehaviour, IDigCompletedHandler
    {

        private const int MAX_RESOURCES_IN_STACK = 10;
        private const int MAX_ITEMS_IN_STACK = 10;
        private const int MINIMUM_MONEY_REWARD = 1000;
        [SerializeField] private GameEvent _tryChangeResourcesInPlayerCargo = null;
        [SerializeField] private GameEvent _changeUsableItems = null;
        [SerializeField] private GameEvent _repairPlayer = null;
        [SerializeField] private GroundLayerList _layers = null;
        [SerializeField] private IntReference _playerLayer = null;
        [SerializeField] private UsableItemList _usableItems = null;
        [SerializeField] private IntReference _playerMoney = null;

        public void OnDigCompleted()
        {

            TryChangeResourcesInPlayerCargoEA tcripc = new TryChangeResourcesInPlayerCargoEA();
            ChangeUsableItemsEA cui = new ChangeUsableItemsEA();
            int category = Random.Range(0, 100);
            int value = (int)Mathf.Pow(_playerLayer.Value * 500, 1 + (_playerLayer.Value / 20f));

            if(category < 40) //resources
            {
                tcripc.ResourcesToAdd = SelectResources(10 * value);
                _tryChangeResourcesInPlayerCargo.Raise(tcripc);
            }
            else if(category >= 40 && category < 80) //resources and items
            {
                tcripc.ResourcesToAdd = SelectResources((int)(value / 2f));
                _tryChangeResourcesInPlayerCargo.Raise(tcripc);
                cui.AddedUsableItems = SelectUsableItems((int)(value / 2f));
                _changeUsableItems.Raise(cui);
            }
            else if(category >= 80 && category < 95) //items
            {
                cui.AddedUsableItems = SelectUsableItems(value);
                _changeUsableItems.Raise(cui);
            }
            else if(category >= 95) //extra internal repair
            {
                RepairPlayer();
            }
        }

        private List<CargoTable.Element> SelectResources(int value)
        {
            List<CargoTable.Element> result = new List<CargoTable.Element>();
            List<GroundLayer.Element> elements = _layers[_playerLayer.Value].Resources;

            List<TileType> collectibleTypes = new List<TileType>(elements.Count);
            for(int i = 0; i < elements.Count; ++i)
            {
                if(elements[i].Type.IsCollectible)
                    collectibleTypes.Add(elements[i].Type);
            }

            if(collectibleTypes.Count == 0)
            {
                Management.Log.Instance.WriteException(new InvalidSettingException());
                return result;
            }

            int minimumResourceValue = collectibleTypes.Min(x => x.Value);

            int amountOfSelectedResources = 0;

            while (value >= minimumResourceValue && collectibleTypes.Count > 0)
            {
                int selectedType = Random.Range(0, collectibleTypes.Count);
                int calculatedAmount = 0;

                if (collectibleTypes[selectedType].Value > 0)
                    calculatedAmount = Mathf.Clamp(value / collectibleTypes[selectedType].Value, 1, MAX_RESOURCES_IN_STACK);
                else
                    calculatedAmount = Random.Range(1, MAX_RESOURCES_IN_STACK);

                value -= calculatedAmount * collectibleTypes[selectedType].Value;
                result.Add(new CargoTable.Element() { Type = collectibleTypes[selectedType], Amount = calculatedAmount });
                amountOfSelectedResources++;
                collectibleTypes.RemoveAt(selectedType);
            }

            if (value > MINIMUM_MONEY_REWARD)
                _playerMoney.Value += 100 * (int)(value / 100f);
            if (amountOfSelectedResources == 0)
                _playerMoney.Value += 100 * (int)(Random.Range(0.5f, 1.5f) * value / 100f);

            return result;
        }

        private List<UsableItemTable.Element> SelectUsableItems(int value)
        {
            List<UsableItemTable.Element> result = new List<UsableItemTable.Element>();
            List<UsableItem> possibleItems = new List<UsableItem>(_usableItems.Count);
            foreach(var item in _usableItems)
            {
                possibleItems.Add(item);
            }

            if (possibleItems.Count == 0)
            {
                Management.Log.Instance.WriteException(new InvalidSettingException());
                return result;
            }

            try
            {
                int minimumItemCost = _usableItems.Min(x => x.Cost);
                int amountOfSelectedItems = 0;

                while (value >= minimumItemCost && possibleItems.Count > 0)
                {
                    int selectedType = Random.Range(0, possibleItems.Count);
                    int calculatedAmount = 0;

                    if (possibleItems[selectedType].Cost > 0)
                        calculatedAmount = Mathf.Clamp(value / possibleItems[selectedType].Cost, 1, MAX_ITEMS_IN_STACK);
                    else
                        calculatedAmount = Random.Range(1, MAX_ITEMS_IN_STACK);

                    value -= calculatedAmount * possibleItems[selectedType].Cost;
                    result.Add(new UsableItemTable.Element() { Item = possibleItems[selectedType], Amount = calculatedAmount });
                    amountOfSelectedItems++;
                    possibleItems.RemoveAt(selectedType);
                }

                if (value > MINIMUM_MONEY_REWARD)
                    _playerMoney.Value += 100 * (int)(value / 100f);
                if (amountOfSelectedItems == 0)
                    _playerMoney.Value += 100 * (int)(Random.Range(0.5f, 1.5f) * value / 100f);
            }
            catch(Exception e)
            {
                Management.Log.Instance.WriteException(e);
            }

            return result;
        }

        private void RepairPlayer()
        {
            RepairPlayerEA rp = new RepairPlayerEA();
            int amountOfRepairedParts = Random.Range(1, 3);
            List<EPartType> partTypes = ((EPartType[])Enum.GetValues(typeof(EPartType))).ToList();

            for(int i = 0; i < amountOfRepairedParts; ++i)
            {
                if (partTypes.Count == 0) break;
                EPartType partType = partTypes[Random.Range(0, partTypes.Count)];
                rp.PermaRepair.Add(partType, Random.Range(1, 3) / 100f);
                partTypes.Remove(partType);
            }
            _repairPlayer.Raise(rp);
        }
    }
}