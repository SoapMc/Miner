using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Miner.Management;
using System.Text;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    public class TilemapData : ISaveable, ILoadable
    {
        private int[,] _data;
        private Vector2Int _size;
        private int _minimumDepth;  //signed value

        public TilemapData() { }

        public TilemapData(int width, int height, int maximumSurfaceDepth)
        {
            _size = new Vector2Int(width, height);
            _data = new int[width, height];
            _minimumDepth = maximumSurfaceDepth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">X coord in grid space</param>
        /// <param name="y">Y coord in grid space</param>
        /// <param name="tileId">Identifier of a tile type</param>
        public void SetTileId(int x, int y, int tileId)
        {
            Vector2Int pos = new Vector2Int(Mathf.Clamp(x, 0, _size.x - 1), Mathf.Clamp(-y + _minimumDepth, 0, _size.y - 1));
            _data[pos.x, pos.y] = tileId;
        }

        public void SetTileId(Vector2Int gridPosition, int tileId)
        {
            Vector2Int pos = new Vector2Int(Mathf.Clamp(gridPosition.x, 0, _size.x - 1), Mathf.Clamp(-gridPosition.y + _minimumDepth, 0, _size.y - 1));
            _data[pos.x, pos.y] = tileId;
        }

        public int GetTileId(Vector2Int gridPosition)
        {
            try
            {
                int y = -gridPosition.y + _minimumDepth;

                
                if (gridPosition.x < 0 || gridPosition.x >= _size.x || y < 0 || y >= _size.y)
                    return -1;
                return _data[gridPosition.x, y];
            }
            catch(IndexOutOfRangeException e)
            {
                Log.Instance.Write(GetType().ToString() + " : " + e.Message + " [x: " + gridPosition.x + " /y:" + (-gridPosition.y + _minimumDepth) + "]");
                return -1;
            }
        }

        public int[,] RetrieveSerializableData()
        {
            return _data;
        }

        public void Load(int[,] tilemapData)
        {
            _data = tilemapData;
        }

        public void WriteObjectToStream(StreamWriter stream)
        {
            stream.Write(_size.x + " " + _size.y + " " + _minimumDepth);
            stream.Write(Environment.NewLine);
            for (int y = 0; y < _size.y; ++y)
            {
                for (int x = 0; x < _size.x; ++x)
                {
                    stream.Write(_data[x, y] + " ");
                }
                stream.Write(Environment.NewLine);
            }
        }

        public void LoadFromStream(StreamReader stream)
        {
            try
            {
                string buffer;
                buffer = stream.ReadLine(); //first line
                string[] headerValuesText = buffer.Split(' ');
                if (headerValuesText.Length == 3)
                {
                    int.TryParse(headerValuesText[0], out int sizeX);
                    int.TryParse(headerValuesText[1], out int sizeY);
                    _size = new Vector2Int(sizeX, sizeY);
                    _data = new int[sizeX, sizeY];
                    int.TryParse(headerValuesText[2], out int minimumDepth);
                    _minimumDepth = minimumDepth;
                }

                string[] tiles;
                int depth = _minimumDepth;
                while ((buffer = stream.ReadLine()) != null)
                {
                    tiles = buffer.Split(' ');
                    for (int x = 0; x < _size.x; ++x)
                    {
                        SetTileId(x, depth, int.Parse(tiles[x]));
                    }
                    depth--;
                }
            }
            catch(Exception e)
            {
                Log.Instance.Write(GetType() + " : " + e.Message);
            }
        }
    }
}