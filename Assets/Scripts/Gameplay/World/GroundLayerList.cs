﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Ground Layer List")]
    public class GroundLayerList : ScriptableObject, ICollection<GroundLayer>
    {
        [SerializeField] private List<GroundLayer> _layers = new List<GroundLayer>();

        public int Count => _layers.Count;

        public bool IsReadOnly => false;

        public void Add(GroundLayer item)
        {
            _layers.Add(item);
        }

        public void Clear()
        {
            _layers.Clear();
        }

        public bool Contains(GroundLayer item)
        {
            return _layers.Contains(item);
        }

        public void CopyTo(GroundLayer[] array, int arrayIndex)
        {
            _layers.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GroundLayer> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        public List<GroundLayer> GetRange(int index, int count)
        {
            return _layers.GetRange(index, count);
        }

        public bool Remove(GroundLayer item)
        {
            return _layers.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        /// <summary>
        /// Gets underground layer with specified index.
        /// </summary>
        /// <param name="index">Number of underground layer.</param>
        /// <returns></returns>
        public GroundLayer this[int index]
        {
            get
            {
                try
                {
                    return _layers[index];
                }
                catch(IndexOutOfRangeException e)
                {
                    Log.Instance.Write(GetType() + " : " + e.Message);
                    if(_layers.Count > 0)
                        return _layers[0];
                    return null;
                }
            }
        }
    }
}