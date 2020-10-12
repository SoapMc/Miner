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

   public class CameraTranslatedEA : EventArgs
    {
        public readonly Vector2Int oldGridPosition;
        public readonly Vector2Int newGridPosition;

        public CameraTranslatedEA(Vector2Int oldPos, Vector2Int newPos)
        {
            oldGridPosition = oldPos;
            newGridPosition = newPos;
        }
    }

    public class ChangeAlarmsEA : EventArgs
    {
        public AlarmDisplay.EAlarmType? AddedAlarm;
        public AlarmDisplay.EAlarmImportance AddedAlarmImportance = AlarmDisplay.EAlarmImportance.Warning;
        public AlarmDisplay.EAlarmType? RemovedAlarm;
    }

    public class CloseWindowEA : EventArgs
    {
        public readonly Window ClosedWindow;

        public CloseWindowEA(Window closedWindow)
        {
            ClosedWindow = closedWindow;
        }
    }

    public class CreateMessageEA : EventArgs
    {
        public readonly string Title;
        public readonly string Message;
        public readonly Message.EType Type;
        public readonly float Time = 10f;
        
        public CreateMessageEA(string title, string message, Message.EType type = UI.Message.EType.Statement)
        {
            Title = title;
            Message = message;
            Type = type;
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
        public readonly Window WindowPrefab;

        public CreateWindowEA(Window windowPrefab)
        {
            WindowPrefab = windowPrefab;
        }
    }

    public class DescriptElementEA : EventArgs
    {
        public readonly IDescriptableElement DescriptableElement;
        public readonly RectTransform RectTransform;

        public DescriptElementEA(IDescriptableElement element, RectTransform rectTransform)
        {
            DescriptableElement = element;
            RectTransform = rectTransform;
        }
    }

    public class DescriptOfferEA : EventArgs
    {
        public readonly IDescriptableElement DescriptableElement;
        public readonly RectTransform RectTransform;
        public readonly EState State;

        public DescriptOfferEA(IDescriptableElement element, RectTransform rectTransform, EState state)
        {
            DescriptableElement = element;
            RectTransform = rectTransform;
            State = state;
        }

        public enum EState
        {
            Available,
            Unavailable,
            Bought,
            Locked
        }
    }

    public class DestroyTilesEA : EventArgs
    {
        public List<Vector2Int> Coordinates = null;
        public int DestructionPower;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates">Grid coordinates.</param>
        /// <param name="destructionPower">Destruction power is tested against tile's hardiness. If this parameter has higher value, tile is successfully destroyed.</param>
        public DestroyTilesEA(List<Vector2Int> coordinates, int destructionPower)
        {
            Coordinates = coordinates;
            DestructionPower = destructionPower;
        }
    }

    public class EarthquakeEA : EventArgs
    {
        public readonly float Duration;
        public readonly float Intensity;

        public EarthquakeEA(float duration, float intensity)
        {
            Duration = duration;
            Intensity = intensity;
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

    public class OverrideAmbientLightEA : EventArgs
    {
        public Color Color;
        public bool Surface;
        public bool Underground;

        public OverrideAmbientLightEA(Color col, bool surface, bool underground)
        {
            Color = col;
            Surface = surface;
            Underground = underground;
        }
    }


    public class OverrideSkyEA : EventArgs
    {
        public GameObject Sky;

        public OverrideSkyEA(GameObject sky)
        {
            Sky = sky;
        }

        public static OverrideSkyEA Default => new OverrideSkyEA(null);
    }
    

    public class OverrideTilesEA : EventArgs
    {
        public Dictionary<Vector2Int, TileType> Coordinates = new Dictionary<Vector2Int, TileType>();

        public OverrideTilesEA(Dictionary<Vector2Int, TileType> coords)
        {
            Coordinates = coords;
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

    

    public class UpdateInfrastructureEA : EventArgs
    {
        public int FuelSupplyChange;
    }

    public class WindowCreatedEA : EventArgs
    {
        public readonly Window CreatedWindow;

        public WindowCreatedEA(Window createdWindow)
        {
            CreatedWindow = createdWindow;
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