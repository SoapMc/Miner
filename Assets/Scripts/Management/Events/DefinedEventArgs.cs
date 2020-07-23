﻿using System.Collections;
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
    public class AddResourceToCargoEA : EventArgs
    {
        public readonly CargoTable.Element Resource;
        public bool IsLost;

        public AddResourceToCargoEA(CargoTable.Element resource, bool isLost = false)
        {
            Resource = resource;
            IsLost = isLost;
        }
    }

    public class CameraShakeEA : EventArgs
    {
        public readonly float Amplitude;
        public readonly float VibrationRate;
        public readonly float Damping;

        public CameraShakeEA(float amplitude, float vibrationRate = 60f, float damping = 0.05f)
        {
            Amplitude = amplitude;
            VibrationRate = vibrationRate;
            Damping = Mathf.Clamp01(damping);
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

    public class CloseWindowEA : EventArgs
    {
        public readonly GameObject ClosedWindow;

        public CloseWindowEA(GameObject closedWindow)
        {
            ClosedWindow = closedWindow;
        }
    }

    public class CreateParticleEA : EventArgs
    {
        public readonly ParticleSystem Particle;
        public readonly Vector2 Position;
        public readonly ECoordinateType CoordinateType;

        public CreateParticleEA(ParticleSystem particle, Vector2 position, ECoordinateType coordType = ECoordinateType.World)
        {
            Particle = particle;
            Position = position;
            CoordinateType = coordType;
        }

        public enum ECoordinateType
        {
            World,
            Grid
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

    public class DestroyTilesEA : EventArgs
    {
        public List<Vector2Int> Coordinates = null;

        public DestroyTilesEA(List<Vector2Int> coordinates)
        {
            Coordinates = coordinates;
        }
    }

    public class DigRequestEA : EventArgs
    {
        public readonly Vector2Int Coordinates;
        public readonly float DrillSharpness;
        public readonly Vector3 PlayerPosition;

        public DigRequestEA(Vector2Int coordinates, float drillSharpness, Vector3 playerPosition)
        {
            Coordinates = coordinates;
            DrillSharpness = drillSharpness;
            PlayerPosition = playerPosition;
        }
    }

    public class LayerTriggerEA : EventArgs
    {
        public readonly int LayerNumber;
        public readonly bool LayerActivation;

        public LayerTriggerEA(int layerNumber, bool layerActivation)
        {
            LayerNumber = layerNumber;
            LayerActivation = layerActivation;
        }
    }

    public class LeadToDigPlaceEA : EventArgs
    {
        public readonly TileType Tile;
        public readonly Vector2Int Place;
        public readonly float Speed;
        public readonly float Hardness;
        public readonly Vector3 PlayerPosition;

        public LeadToDigPlaceEA(TileType tile, Vector2Int place, float speed, float hardness, Vector3 playerPosition)
        {
            Tile = tile;
            Place = place;
            Speed = speed;
            Hardness = hardness;
            PlayerPosition = playerPosition;
        }
    }

    public class MovePlayerEA : EventArgs
    {
        public readonly Vector2 Position;

        public MovePlayerEA(Vector2 position)
        {
            Position = position;
        }
    }

    public class PlayMusicEA : EventArgs
    {
        public readonly AudioClip Music;

        public PlayMusicEA(AudioClip music)
        {
            Music = music;
        }
    }

    public class PlaySoundEA : EventArgs
    {
        public readonly SoundEffect SFX;
        public readonly AudioSource Target;

        public PlaySoundEA(SoundEffect sfx, AudioSource target = null)
        {
            SFX = sfx;
            Target = target;
            if (SFX.Loop == true && target == null)
            {
                Debug.LogError("SFX cannot be looped when using global audio source!");
            }
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

    public class RestoreGameAfterPlayerDestroyedEA : EventArgs
    {
        public readonly Transform PlayerSpawnPoint;

        public RestoreGameAfterPlayerDestroyedEA(Transform playerSpawnPoint)
        {
            PlayerSpawnPoint = playerSpawnPoint;
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

        public bool IsEmpty()
        {
            if (EnableIcons.Count == 0 && DisableIcons.Count == 0)
                return true;
            return false;
        }

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

    public class UpdateInfrastructureEA : EventArgs
    {
        public int FuelSupplyChange;
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
        public int HullPermaDamage = 0;
        public int FuelTankPermaDamage = 0;
        public int EnginePermaDamage = 0;
        public int CoolingPermaDamage = 0;
        public int DrillPermaDamage = 0;
        public int CargoPermaDamage = 0;
        public int BatteryPermaDamage = 0;
        public readonly List<CargoTable.Element> AddCargoChange = new List<CargoTable.Element>();
        public readonly List<CargoTable.Element> RemoveCargoChange = new List<CargoTable.Element>();
        public readonly List<ReferencePart> EquipmentChange = new List<ReferencePart>();
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

    public class WorldLoadedEA : EventArgs
    {
        public readonly Grid WorldGrid;
        public readonly Transform PlayerSpawnPoint;

        public WorldLoadedEA(Grid worldGrid, Transform playerSpawnPoint)
        {
            WorldGrid = worldGrid;
            PlayerSpawnPoint = playerSpawnPoint;
        }
    }
}