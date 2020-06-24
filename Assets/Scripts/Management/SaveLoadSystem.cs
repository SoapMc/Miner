using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.IO;

namespace Miner.Management
{
    public static class SaveLoadSystem
    {
        private const string _directoryName = "/My Games/Miner/";

        public static void SaveToFile<T>(T obj, string fileName)
        {
            if (!Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName))
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName);
            using (StreamWriter file = new StreamWriter(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName + fileName))
            {
                string json = JsonUtility.ToJson((T)obj, true);
                file.Write(json);
            }
        }

        public static bool LoadFromFile<T>(ref T obj, string fileName)
        {
            try
            { 
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName + fileName;
                if (File.Exists(path))
                {
                    using (StreamReader file = new StreamReader(path))
                    {
                        obj = JsonUtility.FromJson<T>(file.ReadToEnd());
                    }
                    return true;
                }
                else
                    return false; //file not found
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveSaveState(string fileName)
        {
            try
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName + fileName;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}