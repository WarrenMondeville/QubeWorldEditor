using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace QubeWorld
{
    [Serializable]
    public class Terrain
    {
        public byte[,,] map;
        public byte[,,] sty;
    }
    public class TerrainDataCtr
    {
        public static void SaveData(byte[] data, string name)
        {
            var path = GetPath(name);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
            formatter.Serialize(file, data);
            file.Close();
        }

        public static byte[] LoadData(string name)
        {
            var path = GetPath(name);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                var saveData = (byte[])formatter.Deserialize(file);
                file.Close();

                return saveData;
            }
            return null;
        }

        public static void SaveDataMap(byte[,,] data, string name)
        {
            var path = GetPath(name);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
            formatter.Serialize(file, data);
            file.Close();
        }

        public static byte[,,] LoadDataMap(string name)
        {
            var path = GetPath(name);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                var saveData = (byte[,,])formatter.Deserialize(file);
                file.Close();
                return saveData;
            }
            return null;
        }

        public static void SaveTerrain(Terrain data, string name)
        {
            var path = GetPath(name);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
            formatter.Serialize(file, data);
            file.Close();
        }

        public static Terrain LoadTerrain(string name)
        {
            var path = GetPath(name);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                var saveData = (Terrain)formatter.Deserialize(file);
                file.Close();
                return saveData;
            }
            return null;
        }

        private static string GetPath(string name)
        {
            return $"{Application.streamingAssetsPath}/Terrian/{name}";
        }

    }

}