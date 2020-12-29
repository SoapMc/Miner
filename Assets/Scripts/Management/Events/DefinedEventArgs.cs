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
    public class AcceptMissionEA : EventArgs
    {
        public readonly Mission Mission;

        public AcceptMissionEA(Mission mission) { Mission = mission; }
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

    public class CancelMissionEA : EventArgs
    {
        public readonly Mission Mission;

        public CancelMissionEA(Mission mission) { Mission = mission; }
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
        public readonly EOfferState State;

        public DescriptOfferEA(IDescriptableElement element, RectTransform rectTransform, EOfferState state)
        {
            DescriptableElement = element;
            RectTransform = rectTransform;
            State = state;
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

    public class MissionCompletedEA : EventArgs
    {
        public readonly Mission Mission;

        public MissionCompletedEA(Mission mission)
        {
            Mission = mission;
        }
    }

    public class MissionFailedEA : EventArgs
    {
        public readonly Mission Mission;

        public MissionFailedEA(Mission mission)
        {
            Mission = mission;
        }
    }

    public class ChangeAmbientLightEA : EventArgs
    {
        public AmbientLightSetting SurfaceLighting = null;
        public AmbientLightSetting UndergroundLighting = null;

        public enum EChangeMode
        {
            Override,
            Stack
        }

        public class AmbientLightSetting
        {
            public AmbientLight LightToAdd = null;
            public EChangeMode ChangeMode = EChangeMode.Stack;
            public AmbientLight LightToRemove = null;
        }
    }


    public class ChangeSkyEA : EventArgs
    {
        public Sky Sky;

        public ChangeSkyEA(Sky sky)
        {
            Sky = sky;
        }
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

    public class ShowBriefInfoEA : EventArgs
    {
        public readonly string Message;
        public readonly EType Type;

        public ShowBriefInfoEA(string message, EType type = EType.Info)
        {
            Message = message;
            Type = type;
        }

        public enum EType
        {
            Info,
            Warning
        }
    }

    public class ShowInputHelpEA : EventArgs
    {
        public readonly Dictionary<EInputType, string> Elements = null;

        public ShowInputHelpEA(Dictionary<EInputType, string> inputHelpFields)
        {
            Elements = inputHelpFields;
        }
    }

    public class ShowPartDescriptionEA : EventArgs
    {
        public readonly ReferencePart Part;
        public readonly EOfferState State;

        public ShowPartDescriptionEA(ReferencePart part, EOfferState state)
        {
            Part = part;
            State = state;
        }
    }

    public class UpdateInfrastructureEA : EventArgs
    {
        public int FuelSupplyChange;
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