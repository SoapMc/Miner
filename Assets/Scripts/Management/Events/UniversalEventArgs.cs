using System.Collections.Generic;
using System;
using UnityEngine;
using Miner.Management;

namespace Miner.Management.Events
{

    public class UniversalEventArgs : EventArgs
    {
        private Dictionary<string, string> _arguments = new Dictionary<string, string>();

        public UniversalEventArgs(params Tuple<string, string>[] args)
        {
            for(int i = 0; i < args.Length; ++i)
            {
                AddArgument(args[i].Item1, args[i].Item2);
            }
        }

        private void AddArgument(string name, string value)
        {
            if(!_arguments.ContainsKey(name))
            {
                _arguments.Add(name, value);
            }
            else
            {
                GameManager.Instance.Log.WriteException(new ArgumentException());
            }
        }

        public T GetArgument<T>(string name)
        {
            try
            {
                return (T)Convert.ChangeType(_arguments[name], typeof(T));
            }
            catch (Exception e)
            {
                GameManager.Instance.Log.WriteException(e);
                return (T)Convert.ChangeType(0, typeof(T));
            }
        }

        public int GetInt(string name)
        {
            try
            {
                return int.Parse(_arguments[name]);
            }
            catch(Exception e)
            {
                GameManager.Instance.Log.WriteException(e);
                return 0;
            }
        }

        public float GetFloat(string name)
        {
            try
            {
                return float.Parse(_arguments[name]);
            }
            catch (Exception e)
            {
                GameManager.Instance.Log.WriteException(e);
                return 0f;
            }
        }

        public string GetString(string name)
        {
            try
            {
                return _arguments[name];
            }
            catch (Exception e)
            {
                GameManager.Instance.Log.WriteException(e);
                return string.Empty;
            }
        }
    }
}