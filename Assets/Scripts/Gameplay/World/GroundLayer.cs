using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Ground Layer")]
    public class GroundLayer : ScriptableObject
    {
        [SerializeField] private GameEvent _playMusic = null;
        public int LayerNumber = 0;
        public EAreaType AreaType = default;
        public int Depth = 0;   //always positive value
        [Range(0f, 1f)] public float ProbabilityOfEmptySpaces = 0.01f;
        public Color BackgroundColor = Color.white;
        public Color AmbientLightColor = Color.black;
        public List<PredefinedWorldObject> PredefinedWorldObjects = new List<PredefinedWorldObject>();
        public List<NaturalDisaster> NaturalDisasters = new List<NaturalDisaster>();
        public List<AudioClip> Music = new List<AudioClip>();
        public TileType DefaultTile = null;
        public List<Element> Resources = new List<Element>();

        public float TotalResourceProbability => Resources.Sum(x => x.Probability);

        public List<float> GetResourceProbabilitiesForGeneration()
        {
            List<float> result = new List<float>(Resources.Count);
            float prob = 0f;
            for (int i = 0; i < Resources.Count; ++i)
            {
                prob += Resources[i].Probability;
                result.Add(prob);
            }
            return result;
        }

        public void PlayMusic()
        {
            if(Music.Count > 0)
            {
                _playMusic.Raise(new PlayMusicEA(Music[Random.Range(0, Music.Count)]));
            }
        }

        [System.Serializable]
        public class Element
        {
            public TileType Type;
            [Range(0f, 1f)] public float Probability;
        }

        public enum EAreaType
        {
            Surface,
            Underground
        }
    }
}