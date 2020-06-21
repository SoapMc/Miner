using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using Miner.Gameplay;
using Miner.UI;

namespace Miner.Management.Events
{
    public class CloseWindowEA : EventArgs
    {
        public readonly GameObject ClosedWindow;

        public CloseWindowEA(GameObject closedWindow)
        {
            ClosedWindow = closedWindow;
        }
    }

    public class CreateWindowEA : EventArgs
    {
        public readonly GameObject WindowPrefab;

        public CreateWindowEA(GameObject windowPrefab)
        {
            WindowPrefab = windowPrefab;
        }
    }

    public class DigRequestEA : EventArgs
    {
        public readonly Vector2Int Coordinates;
        
        public DigRequestEA(Vector2Int coordinates)
        {
            Coordinates = coordinates;
        }
    }

    public class LeadToDigPlaceEA : EventArgs
    {
        public readonly TileType Tile;
        public readonly Vector2Int Place;
        public readonly float Speed;
        public readonly float Hardness;

        public LeadToDigPlaceEA(TileType tile, Vector2Int place, float speed, float hardness)
        {
            Tile = tile;
            Place = place;
            Speed = speed;
            Hardness = hardness;
        }
    }

    public class ShowPartDescriptionEA : EventArgs
    {
        public readonly ReferencePart Part;
        public readonly PartGridElement.State State;

        public ShowPartDescriptionEA(ReferencePart part, PartGridElement.State state)
        {
            Part = part;
            State = state;
        }
    }

    public class TriggerStatusPanelEA : EventArgs
    {
        public List<Element> EnableIcons = new List<Element>();
        public List<ESymbol> DisableIcons = new List<ESymbol>();

        public class Element
        {
            public ESymbol Symbol;
            public EMode Mode;
            public float Time; //in seconds, 0 means unlimited time
        }

        public enum ESymbol
        {
            Engine,
            Temperature,
            Fuel,
            Cargo,
            Drill,
            Battery
        }

        public enum EMode
        {
            Warning,
            Failure
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
        public int HullChange = 0;
        public int MaxHullChange = 0;
        public float FuelChange = 0f;
        public float MaxFuelChange = 0f;
        public readonly List<CargoTable.Element> AddCargoChange = new List<CargoTable.Element>();
        public readonly List<CargoTable.Element> RemoveCargoChange = new List<CargoTable.Element>();
        public readonly List<ReferencePart> EquipmentChange = new List<ReferencePart>();
        public readonly List<UsableItemTable.Element> AddUsableItemsChange = new List<UsableItemTable.Element>();
        public readonly List<UsableItemTable.Element> RemoveUsableItemsChange = new List<UsableItemTable.Element>();
        
    }

    public class WorldLoadedEA : EventArgs
    {
        public readonly Tilemap Tilemap;
        public readonly Transform PlayerSpawnPoint;

        public WorldLoadedEA(Tilemap tilemap, Transform playerSpawnPoint)
        {
            Tilemap = tilemap;
            PlayerSpawnPoint = playerSpawnPoint;
        }
    }
}