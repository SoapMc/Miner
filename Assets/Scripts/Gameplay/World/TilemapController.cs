using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Miner.Gameplay
{
    public class TilemapController : MonoBehaviour
    {
        private List<Tuple<int, GroundLayerController>> _tilemaps = new List<Tuple<int, GroundLayerController>>();

        public void AddTilemap(GroundLayerController tilemap, int maximumLayerDepth)
        {
            _tilemaps.Add(new Tuple<int, GroundLayerController>(maximumLayerDepth, tilemap));
        }

        public Tile GetTile(Vector2Int pos)
        {
            return _tilemaps.FirstOrDefault(x => x.Item1 < pos.y)?.Item2.Tilemap.GetTile<Tile>((Vector3Int)pos);
        }

        public void SetTile(Vector2Int pos, Tile tile)
        {
            _tilemaps.First(x => x.Item1 < pos.y).Item2.Tilemap.SetTile((Vector3Int)pos, tile);
        }

        public void OnTriggerUpperLayer(EventArgs args)
        {
            if(args is LayerTriggerEA lt)
            {
                int triggeredLayer = lt.LayerNumber - 1;
                if (triggeredLayer > 0 && triggeredLayer < _tilemaps.Count && _tilemaps[triggeredLayer].Item2.gameObject.activeSelf != lt.LayerActivation)
                    _tilemaps[triggeredLayer].Item2.gameObject.SetActive(lt.LayerActivation);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnTriggerBottomLayer(EventArgs args)
        {
            if (args is LayerTriggerEA lt)
            {
                int triggeredLayer = lt.LayerNumber + 1;
                if (triggeredLayer > 0 && triggeredLayer < _tilemaps.Count && _tilemaps[triggeredLayer].Item2.gameObject.activeSelf != lt.LayerActivation)
                        _tilemaps[triggeredLayer].Item2.gameObject.SetActive(lt.LayerActivation);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
    }
}