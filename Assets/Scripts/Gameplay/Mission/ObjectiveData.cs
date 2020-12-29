using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management;

namespace Miner.Gameplay
{

    [System.Serializable]
    public class ObjectiveData
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private EMissionObjectiveType _type = default;
        [SerializeField] private List<Field> _fields = new List<Field>();

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public EMissionObjectiveType Type => _type;

        public void AddField(string name, int value)
        {
            for (int i = 0; i < _fields.Count; ++i)
            {
                if (_fields[i].Name == name)
                    return;
            }
            _fields.Add(new Field() { Name = name, Value = value });
        }

        public int GetFieldValue(string name)
        {
            for (int i = 0; i < _fields.Count; ++i)
            {
                if (_fields[i].Name == name)
                    return _fields[i].Value;
            }
            Log.Instance.Write(GetType() + " : " + new KeyNotFoundException() + " (name: " + name + ")");
            return 0;
        }

        public void SetFieldValue(string name, int value)
        {
            for (int i = 0; i < _fields.Count; ++i)
            {
                if (_fields[i].Name == name)
                {
                    _fields[i].Value = value;
                    return;
                }
            }
            Log.Instance.WriteException(new KeyNotFoundException(name));
        }

        [System.Serializable]
        public class Field
        {
            public string Name;
            public int Value;
        }
    }
}