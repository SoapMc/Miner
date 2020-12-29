using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Mission/Gather Resources Objective")]
    public class GatherResourcesObjectiveDefinition : ObjectiveDefinition
    {
        private const int MAX_RESOURCE_STACK = 20;
        [SerializeField] private GameEvent _playerResourcesGathered = null;
        [SerializeField] private IntReference _playerMaxAchievedLayer = null;
        [SerializeField] private GroundLayerList _groundLayers = null;
        [SerializeField] private TileTypes _tileTypes = null;
        
        public override Objective CreateObjective()
        {
            ObjectiveData objectiveData = new ObjectiveData();
            List<GroundLayer.Element> collectibleResources = new List<GroundLayer.Element>(_groundLayers[_playerMaxAchievedLayer.Value].Resources.Count);
            foreach(GroundLayer.Element elem in _groundLayers[_playerMaxAchievedLayer.Value].Resources)
            {
                if(elem.Type.IsCollectible)
                    collectibleResources.Add(elem);
            }

            if(collectibleResources.Count == 0)
            {
                Management.Log.Instance.WriteException(new ExecutionException());
                return null;
            }

            GroundLayer.Element selectedResource = collectibleResources[Random.Range(0, collectibleResources.Count)];

            float difficultyCoefficient = 1f;
            if (selectedResource.Probability != 0f)
            {
                difficultyCoefficient /= selectedResource.Probability;
            }

            int requiredAmount = (int)Mathf.Clamp(Random.Range(0.8f, 1.2f) * 10 *  difficultyCoefficient/100f, 1 , MAX_RESOURCE_STACK);
            objectiveData.AddField("RequiredResourceId", selectedResource.Type.Id);
            objectiveData.AddField("CollectedResourceAmount", 0);
            objectiveData.AddField("RequiredResourceAmount", requiredAmount);
            objectiveData.Name = "Collect resources";
            GatherResourcesObjective result = new GatherResourcesObjective(objectiveData, selectedResource.Type, _playerResourcesGathered);
            return result;
        }

        public override Objective LoadObjective(ObjectiveData data)
        {
            int requiredResourceId = data.GetFieldValue("RequiredResourceId");
            GatherResourcesObjective result = new GatherResourcesObjective(data, _tileTypes.GetTileType(requiredResourceId), _playerResourcesGathered);
            return result;
        }

        /*
        private List<Tuple<int, int>> CalculateResourceValues()
        {

            int indexOfDeepestVisitedLayer = _playerMaxAchievedLayer.Value;

            List<GroundLayer> selectedLayers = _groundLayers.GetRange(0, _playerMaxAchievedLayer.Value + 1);
            List<TileType> tileTypes = new List<TileType>();

            int minLayer = Mathf.Clamp(_playerMaxAchievedLayer.Value - 3, 0, _playerMaxAchievedLayer.Value);
            int maxLayer = _playerMaxAchievedLayer.Value;
            for(int i = minLayer; i <= maxLayer; ++i)
            {

            }

            for (int layerIndex = 0; layerIndex < selectedLayers.Count; ++layerIndex)
            {
                for (int resourceIndex = 0; resourceIndex < selectedLayers[layerIndex].Resources.Count; ++resourceIndex)
                {
                    if (selectedLayers[layerIndex].Resources[resourceIndex].Type.IsCollectible)
                    {
                        if (!tileTypes.Contains(selectedLayers[layerIndex].Resources[resourceIndex].Type))
                            tileTypes.Add(selectedLayers[layerIndex].Resources[resourceIndex].Type);
                    }
                }
            }

            List<Tuple<int, int>> result = new List<Tuple<int, int>>(tileTypes.Count);

            for (int i = 0; i < tileTypes.Count; ++i)
            {
                float maxRarity = 0f;
                int searchedId = tileTypes[i].Id;
                for (int layerIndex = 0; layerIndex < selectedLayers.Count; ++layerIndex)
                {
                    var elem = selectedLayers[layerIndex].Resources.FirstOrDefault(x => x.Type.Id == searchedId);
                    if (elem == null) continue;
                    if (elem.Probability > maxRarity)
                        maxRarity = elem.Probability;
                }
                if (maxRarity > 0f)
                    result.Add(new Tuple<int, int>(tileTypes[i].Id, (int)(1f / maxRarity)));
                else
                    result.Add(new Tuple<int, int>(tileTypes[i].Id, int.MaxValue));
            }

            return result;
        }
        */
    }
}