using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Ground Layer")]
    public class GroundLayer : ScriptableObject
    {
        [SerializeField] private GameEvent _playMusic = null;
        public int LayerNumber = 0;
        public int Depth = 0;
        [Range(0f, 1f)] public float ProbabilityOfEmptySpaces = 0.01f;
        public Color _backgroundColor = Color.white;
        public List<AudioClip> Music = new List<AudioClip>();
        public List<TileType> DefaultTiles = new List<TileType>();
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
    }
}