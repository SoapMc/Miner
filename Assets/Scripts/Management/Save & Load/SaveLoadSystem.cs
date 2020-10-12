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

        public static void SaveToFile<T>(T obj, string fileName, EFormat format = EFormat.Json)
        {
            if (format == EFormat.Json)
            {
                if (!Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName))
                    Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName);
                using (StreamWriter file = new StreamWriter(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName + fileName))
                {
                    string json = JsonUtility.ToJson((T)obj, true);
                    file.Write(json);
                }
            }
            else if(format == EFormat.Txt)
            {
                if (obj is ISaveable saveable)
                {
                    using (StreamWriter file = new StreamWriter(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName + fileName))
                    {
                        saveable.WriteObjectToStream(file);
                    }
                }
            }
        }

        public static bool LoadFromFile<T>(ref T obj, string fileName, EFormat format = EFormat.Json) where T : new()
        {
            try
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + _directoryName + fileName;
                
                if (File.Exists(path))
                {
                    if (format == EFormat.Json)
                    {
                        using (StreamReader file = new StreamReader(path))
                        {
                            obj = JsonUtility.FromJson<T>(file.ReadToEnd());
                        }
                        return true;
                    }
                    else if (format == EFormat.Txt)
                    {
                        obj = new T();
                        if (obj is ILoadable loadable)
                        {
                            using (StreamReader file = new StreamReader(path))
                            {
                                loadable.LoadFromStream(file);
                            }
                            return true;
                        }
                        return false;
                    }
                    else
                        return false;
                }
                else
                    return false; //file not found
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveFile(string fileName)
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

        public enum EFormat
        {
            Json,
            Txt
        }
    }
}