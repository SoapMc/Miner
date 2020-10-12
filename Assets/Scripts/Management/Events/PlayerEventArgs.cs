using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using Miner.Gameplay;
using Miner.UI;
using Miner.FX;
using Miner.Management.Exceptions;

namespace Miner.Management.Events
{
    public class AllowDigEA : EventArgs
    {
        public readonly TileType Tile;
        public readonly Vector2Int Place;
        public readonly float Speed;
        public readonly float Hardness;
        public readonly Vector3 PlayerPosition;
        public readonly EDigDirection Direction;

        public AllowDigEA(TileType tile, Vector2Int place, float speed, float hardness, Vector3 playerPosition, EDigDirection direction)
        {
            Tile = tile;
            Place = place;
            Speed = speed;
            Hardness = hardness;
            PlayerPosition = playerPosition;
            Direction = direction;
        }
    }

    public class ChangePlayerRadiationEA : EventArgs
    {
        public readonly int RadiationChange;

        public ChangePlayerRadiationEA(int radiationChange)
        {
            RadiationChange = radiationChange;
        }
    }

    public class ChooseUsableItemEA : EventArgs
    {
        public readonly UsableItem Item;

        public ChooseUsableItemEA(UsableItem item)
        {
            Item = item;
        }
    }

    public class KillPlayerEA : EventArgs
    {
        public readonly ESource Source;

        public KillPlayerEA(ESource source)
        {
            Source = source;
        }

        public enum ESource
        {
            Damage,
            FuelEnded
        }
    }

    public class TranslatePlayerEA : EventArgs
    {
        public readonly Vector2 Position;

        public TranslatePlayerEA(Vector2 position)
        {
            Position = position;
        }
    }

    public class PlayerCameToLayerEA : EventArgs
    {
        public readonly int LayerNumber;

        public PlayerCameToLayerEA(int layerNumber)
        {
            LayerNumber = layerNumber;
        }
    }

    public class PlayerRadiationChangedEA : EventArgs
    {
        public readonly int Radiation;
        
        public PlayerRadiationChangedEA(int radiation)
        {
            Radiation = radiation;
        }
    }

    public class DamagePlayerEA : EventArgs
    {
        public readonly int Damage;
        public readonly DamageType Type;
        public Dictionary<EPartType, int> PermaDamage = new Dictionary<EPartType, int>();

        public DamagePlayerEA() { }

        public DamagePlayerEA(int damage, DamageType type)
        {
            Damage = damage;
            Type = type;
        }

        public void DealPermaDamage(params Tuple<EPartType, int>[] permaDamages)
        {
            foreach(var tuple in permaDamages)
            {
                if (!PermaDamage.ContainsKey(tuple.Item1))
                    PermaDamage.Add(tuple.Item1, tuple.Item2);
            }
        }
    }

    public class PlayerRepairedEA : EventArgs
    {
        public readonly int Repair;

        public PlayerRepairedEA(int repair)
        {
            Repair = repair;
        }
    }

    public class PlayerTranslatedEA : EventArgs
    {
        public Vector2 OldPosition;
        public Vector2 NewPostion;
        public Vector2Int OldGridPosition;
        public Vector2Int NewGridPostion;

        public PlayerTranslatedEA(Vector2 oldPosition, Vector2 newPosition, Vector2Int oldGridPosition, Vector2Int newGridPosition)
        {
            OldPosition = oldPosition;
            NewPostion = newPosition;
            OldGridPosition = oldGridPosition;
            NewGridPostion = newGridPosition;
        }
    }

    public class PlayerLoadedEA : EventArgs
    {
        public readonly GameObject Player;

        public PlayerLoadedEA(GameObject player)
        {
            Player = player;
        }
    }

    public class ResourcesGatheredEA : EventArgs
    {
        public readonly List<CargoTable.Element> Resources;

        public ResourcesGatheredEA(List<CargoTable.Element> resources)
        {
            Resources = resources;
        }
    }

    public class ResourcesLostEA : EventArgs
    {
        public readonly List<CargoTable.Element> Resources;

        public ResourcesLostEA(List<CargoTable.Element> resources)
        {
            Resources = resources;
        }
    }

    public class ResourcesRemovedEA : EventArgs
    {
        public readonly List<CargoTable.Element> Resources;

        public ResourcesRemovedEA(List<CargoTable.Element> resources)
        {
            Resources = resources;
        }
    }

    public class TryChangeResourcesInPlayerCargoEA : EventArgs
    {
        public List<CargoTable.Element> ResourcesToAdd = new List<CargoTable.Element>();
        public List<CargoTable.Element> ResourcesToRemove = new List<CargoTable.Element>();
    }

    public class TryDigEA : EventArgs
    {
        public readonly Vector2Int GridCoordinates;
        public readonly float DrillSharpness;
        public readonly int DrillPower;
        public readonly Vector3 StartPosition;
        public readonly EDigDirection Direction;

        public TryDigEA(Vector2Int gridCoordinates, float drillSharpness, int drillPower, Vector3 startPosition, EDigDirection direction)
        {
            GridCoordinates = gridCoordinates;
            DrillSharpness = drillSharpness;
            DrillPower = drillPower;
            StartPosition = startPosition;
            Direction = direction;
        }
    }

    public class UpdateCargoTableEA : EventArgs
    {
        public List<CargoTable.Element> AddedResources = new List<CargoTable.Element>();
        public List<CargoTable.Element> RemovedResources = new List<CargoTable.Element>();
    }

    /// <summary>
    /// This class should be used for update only when there is no frequent changes in the stats.
    /// </summary>
    public class UpdatePlayerDataEA : EventArgs
    {
        public int MoneyChange = 0;
        public int MaxHullChange = 0;
        public float FuelChange = 0f;
        public float MaxFuelChange = 0f;
        public int HullPermaDamage = 0;
        public int FuelTankPermaDamage = 0;
        public int EnginePermaDamage = 0;
        public int CoolingPermaDamage = 0;
        public int DrillPermaDamage = 0;
        public int CargoPermaDamage = 0;
        public int BatteryPermaDamage = 0;
        public readonly List<Part> EquipmentChange = new List<Part>();
        public readonly List<UsableItemTable.Element> AddUsableItemsChange = new List<UsableItemTable.Element>();
        public readonly List<UsableItemTable.Element> RemoveUsableItemsChange = new List<UsableItemTable.Element>();

    }

    public class UseItemEA : EventArgs
    {
        public readonly UsableItem Item;

        public UseItemEA(UsableItem item)
        {
            Item = item;
        }
    }
}