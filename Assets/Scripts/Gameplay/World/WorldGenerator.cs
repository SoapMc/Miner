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
        [SerializeField] private Tilemap _surfaceTilemap = null;
        [SerializeField] private Tilemap _undergroundTilemap = null;
        [SerializeField] private LayerBorders _surfaceLayerBorders = null;
        [SerializeField] private LayerBorders _layerBordersPrefab = null;
        [SerializeField] private GroundLayerList _layers = null;
        [SerializeField] private int _ceilHeight = 20;
        [SerializeField] private WorldInfo _worldInfo = null;
        [SerializeField] private Grid _grid = null;
        [SerializeField] private TileTypes _tileTypes = null;
        private TileIdentifier _tileIdentifier = null;

        public void GenerateWorld(WorldController worldController, int seed = -1)
        {
            if (seed != -1)
                Random.InitState(seed);

            _tileIdentifier = new TileIdentifier(_tileTypes);
            int minimumDepthForCurrentLayer = -_layers[0].Depth;
            int maximumDepthForCurrentLayer = -_layers[0].Depth;
            TilemapCollider2D surfaceCollider = _surfaceTilemap.GetComponent<TilemapCollider2D>();
            int worldWidth = Mathf.RoundToInt(surfaceCollider.bounds.size.x);
            _worldInfo.HorizontalBorders = new Vector2Int(Mathf.RoundToInt(surfaceCollider.transform.position.x), Mathf.RoundToInt(surfaceCollider.transform.position.x) + worldWidth);
            int totalDepth = -_layers.Sum(x => x.Depth) - _layers[0].Depth;
            _worldInfo.VerticalBorders = new Vector2Int(_ceilHeight, totalDepth);
            _worldInfo.LoadableGridHorizontalRange = _worldInfo.HorizontalBorders;
            _worldInfo.LoadableGridVerticalRange = new Vector2(totalDepth + 1, -1);
            TilemapData tilemapData = new TilemapData(worldWidth, Mathf.Abs(totalDepth), maximumDepthForCurrentLayer);
            Camera mainCamera = Camera.main;

            for (int x = _worldInfo.HorizontalBorders.x; x < _worldInfo.HorizontalBorders.y; ++x)
            {
                _surfaceTilemap.SetTile(new Vector3Int(x, _ceilHeight, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.SetTile(new Vector3Int(x, totalDepth + 1, 0), _tileEdges.ClasifiedTiles[13]);
            }

            for (int y = _ceilHeight; y > totalDepth; --y)
            {
                _surfaceTilemap.SetTile(new Vector3Int(_worldInfo.HorizontalBorders.x - 1, y, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.SetTile(new Vector3Int(_worldInfo.HorizontalBorders.y, y, 0), _tileEdges.ClasifiedTiles[13]);
            }
            _tilemapController.AddTilemap(_surfaceTilemap, -_layers[0].Depth);
            _tilemapController.AddTilemap(_undergroundTilemap, totalDepth);
            GenerateSurface(tilemapData, _worldInfo.HorizontalBorders.x, _worldInfo.HorizontalBorders.y, minimumDepthForCurrentLayer, minimumDepthForCurrentLayer - _layers[0].Depth);
            maximumDepthForCurrentLayer -= _layers[0].Depth;

            _surfaceLayerBorders.Initialize(_layers[0]);
            for (int i = 1; i < _layers.Count; ++i) //all layers except surface layer (so the loop iterates from 1)
            {
                minimumDepthForCurrentLayer = maximumDepthForCurrentLayer;  //minimum depth is maximum depth from previous layer
                maximumDepthForCurrentLayer -= _layers[i].Depth;
                LayerBorders borders = Instantiate(_layerBordersPrefab, _grid.transform);
                borders.Initialize(_layers[i], minimumDepthForCurrentLayer, worldWidth);
                GenerateLayer(tilemapData, _layers[i], _worldInfo.HorizontalBorders.x, _worldInfo.HorizontalBorders.y, minimumDepthForCurrentLayer, maximumDepthForCurrentLayer);
            }
            
            worldController.Initialize(tilemapData);
        }

        public void LoadWorld()
        {
            _tileIdentifier = new TileIdentifier(_tileTypes);
            int minimumDepthForCurrentLayer = -_layers[0].Depth;
            int maximumDepthForCurrentLayer = -_layers[0].Depth;
            TilemapCollider2D surfaceCollider = _surfaceTilemap.GetComponent<TilemapCollider2D>();
            int worldWidth = Mathf.RoundToInt(surfaceCollider.bounds.size.x);
            _worldInfo.HorizontalBorders = new Vector2Int(Mathf.RoundToInt(surfaceCollider.transform.position.x), Mathf.RoundToInt(surfaceCollider.transform.position.x) + worldWidth);
            int totalDepth = -_layers.Sum(x => x.Depth) - _layers[0].Depth;
            _worldInfo.VerticalBorders = new Vector2Int(_ceilHeight, totalDepth);
            _worldInfo.LoadableGridHorizontalRange = _worldInfo.HorizontalBorders;
            _worldInfo.LoadableGridVerticalRange = new Vector2(totalDepth + 1, -1);
            Camera mainCamera = Camera.main;

            for (int x = _worldInfo.HorizontalBorders.x; x < _worldInfo.HorizontalBorders.y; ++x)
            {
                _surfaceTilemap.SetTile(new Vector3Int(x, _ceilHeight, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.SetTile(new Vector3Int(x, totalDepth + 1, 0), _tileEdges.ClasifiedTiles[13]);
            }

            for (int y = _ceilHeight; y > totalDepth; --y)
            {
                _surfaceTilemap.SetTile(new Vector3Int(_worldInfo.HorizontalBorders.x - 1, y, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.SetTile(new Vector3Int(_worldInfo.HorizontalBorders.y, y, 0), _tileEdges.ClasifiedTiles[13]);
            }

            _tilemapController.AddTilemap(_surfaceTilemap, -_layers[0].Depth);
            _tilemapController.AddTilemap(_undergroundTilemap, totalDepth);
            maximumDepthForCurrentLayer -= _layers[0].Depth;

            _surfaceLayerBorders.Initialize(_layers[0]);
            for (int i = 1; i < _layers.Count; ++i) //all layers except surface layer (so the loop iterates from 1)
            {
                minimumDepthForCurrentLayer = maximumDepthForCurrentLayer;  //minimum depth is maximum depth from previous layer
                maximumDepthForCurrentLayer -= _layers[i].Depth;
                LayerBorders borders = Instantiate(_layerBordersPrefab, _grid.transform);
                borders.Initialize(_layers[i], minimumDepthForCurrentLayer, worldWidth);
            }
        }

        private void GenerateSurface(TilemapData tilemapData, int leftBorder, int rightBorder, int minimumDepth, int maximumDepth)
        {
            for (int x = leftBorder; x < rightBorder; ++x)
            {
                for (int y = minimumDepth; y > maximumDepth; --y)
                {
                    if (_surfaceTilemap.GetTile(new Vector3Int(x, y, 0)) is Tile tile)
                    {
                        tilemapData.SetTileId(x, y, _tileIdentifier.Identify(tile.sprite).Id);
                    }
                }
            }
        }

        private void GenerateLayer(TilemapData tilemapData, GroundLayer groundLayer, int leftBorder, int rightBorder, int minimumDepth, int maximumDepth)
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
                            tilemapData.SetTileId(x, y, groundLayer.Resources[j].Type.Id);
                            tileSet = true;
                            break;
                        }
                    }

                    if (!tileSet)
                    {
                        tilemapData.SetTileId(x, y, groundLayer.DefaultTile.Id);
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
                        tilemapData.SetTileId(x, y, -1);
                    }
                }
            }
        }
    }
}