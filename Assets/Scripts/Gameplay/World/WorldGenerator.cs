using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace Miner.Gameplay
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private TilemapController _tilemapController = null;
        [SerializeField] private TileType _tileEdges = null;
        [SerializeField] private UndergroundTrigger _undergroundTrigger = null;
        [SerializeField] private GroundLayerController _surfaceTilemap = null;
        [SerializeField] private GroundLayerController _groundLayerPrefab = null;
        [SerializeField] private Vector2IntReference _horizontalWorldBorders = null;
        [SerializeField] private Vector2IntReference _vecticalWorldBorders = null;
        [SerializeField] private GroundLayerList _layers = null;
        [SerializeField] private int _ceilHeight = 20;

        public void GenerateWorld(WorldController worldController, int seed = -1)
        {
            if (seed != -1)
                Random.InitState(seed);

            int minimumDepthForCurrentLayer = -1;
            int maximumDepthForCurrentLayer = -_layers[0].Depth;
            TilemapCollider2D surfaceCollider = _surfaceTilemap.GetComponent<TilemapCollider2D>();
            int worldWidth = Mathf.RoundToInt(surfaceCollider.bounds.size.x);
            _horizontalWorldBorders.Value = new Vector2Int(Mathf.RoundToInt(surfaceCollider.transform.position.x), worldWidth);
            int totalDepth = -_layers.Sum(x => x.Depth) - _layers[0].Depth;
            _vecticalWorldBorders.Value = new Vector2Int(_ceilHeight, totalDepth);

            _surfaceTilemap.Initialize(_layers[0], 0, worldWidth, 0);
            Camera mainCamera = Camera.main;

            Tilemap surface = _surfaceTilemap.Tilemap;
            for (int x = _horizontalWorldBorders.Value.x; x < _horizontalWorldBorders.Value.y; ++x)
            {
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(x, _ceilHeight, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(x, totalDepth - 1, 0), _tileEdges.ClasifiedTiles[13]);
            }

            for (int y = _ceilHeight; y > totalDepth; --y)
            {
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(_horizontalWorldBorders.Value.x - 1, y, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(_horizontalWorldBorders.Value.y, y, 0), _tileEdges.ClasifiedTiles[13]);
            }
            _tilemapController.AddTilemap(_surfaceTilemap, -_layers[0].Depth);
            GenerateLayer(worldController, _layers[0], _surfaceTilemap, _horizontalWorldBorders.Value.x, _horizontalWorldBorders.Value.y, -2, -_layers[0].Depth);

            for (int i = 1; i < _layers.Count; ++i)
            {
                minimumDepthForCurrentLayer = maximumDepthForCurrentLayer;  //minimum depth is maximum depth from previous layer
                maximumDepthForCurrentLayer -= _layers[i].Depth;
                GroundLayerController groundLayer = Instantiate(_groundLayerPrefab, worldController.Grid.transform);
                groundLayer.Initialize(_layers[i], minimumDepthForCurrentLayer, worldWidth, mainCamera.orthographicSize);
                _tilemapController.AddTilemap(groundLayer, maximumDepthForCurrentLayer);
                GenerateLayer(worldController, _layers[i], groundLayer, _horizontalWorldBorders.Value.x, _horizontalWorldBorders.Value.y, minimumDepthForCurrentLayer, maximumDepthForCurrentLayer);
            }
        }

        private void GenerateLayer(WorldController worldController, GroundLayer groundLayer, GroundLayerController groundLayerController, int leftBorder, int rightBorder, int minimumDepth, int maximumDepth)
        {
            List<float> resourceProbabilities = groundLayer.GetResourceProbabilitiesForGeneration();
            bool tileSet;
            float prob;


            for (int x = leftBorder; x < rightBorder; ++x)
            {
                for (int y = minimumDepth; y > maximumDepth; --y)
                {
                    tileSet = false;
                    prob = Random.Range(0f, 1f);
                    for (int j = 0; j < groundLayer.Resources.Count; ++j)
                    {
                        if (prob <= resourceProbabilities[j])
                        {
                            groundLayerController.Tilemap.SetTile(new Vector3Int(x, y, 0), groundLayer.Resources[j].Type.ClasifiedTiles[0]);
                            tileSet = true;
                            break;
                        }
                    }

                    if (!tileSet)
                    {
                        groundLayerController.Tilemap.SetTile(new Vector3Int(x, y, 0), groundLayer.DefaultTile.ClasifiedTiles[0]);
                    }

                }
            }

            for (int y = minimumDepth; y > (maximumDepth + 1); --y)
            {
                for (int x = leftBorder; x < rightBorder; ++x)
                {

                    prob = Random.Range(0f, 1f);
                    if (prob <= groundLayer.ProbabilityOfEmptySpaces)
                    {
                        worldController.DestroyTile(new Vector2Int(x, y));
                    }
                }
            }
        }
    }
}