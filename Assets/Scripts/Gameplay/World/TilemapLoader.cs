using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;

namespace Miner.Gameplay
{
    public class TilemapLoader : MonoBehaviour
    {
        [SerializeField] private Vector2IntReference _cameraGridPosition = null;
        [SerializeField] private WorldInfo _worldInfo = null;
        [SerializeField] private Vector2Int _loadedFrameHalfSize = new Vector2Int(5, 5);
        private TilemapController _tilemapController = null;

        public void Initialize(TilemapController controller)
        {
            _tilemapController = controller;
        }

        public void OnChangeCameraGridPosition(Vector2Int oldVal, Vector2Int newVal)
        {
            int dx = newVal.x - oldVal.x;
            int dy = newVal.y - oldVal.y;

            if (dx > 0) //load right area, unload left area
            {
                int yTopPos = (int)Mathf.Clamp(newVal.y + _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                int yBotPos = (int)Mathf.Clamp(newVal.y - _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);

                {
                    //edges of the right frame
                    int xLeftPos = (int)Mathf.Clamp(newVal.x + _loadedFrameHalfSize.x - dx, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    int xRightPos = (int)Mathf.Clamp(newVal.x + _loadedFrameHalfSize.x, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    LoadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }

                {
                    //edges of the left frame
                    int xLeftPos = (int)Mathf.Clamp(newVal.x - _loadedFrameHalfSize.x - dx - 1, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    int xRightPos = (int)Mathf.Clamp(newVal.x - _loadedFrameHalfSize.x, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    UnloadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }
            }
            else if(dx < 0) //load left area, unload right area
            {
                int yTopPos = (int)Mathf.Clamp(newVal.y + _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                int yBotPos = (int)Mathf.Clamp(newVal.y - _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);

                {
                    //edges of the left frame
                    int xLeftPos = (int)Mathf.Clamp(newVal.x - _loadedFrameHalfSize.x, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    int xRightPos = (int)Mathf.Clamp(newVal.x - _loadedFrameHalfSize.x - dx, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    LoadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }

                {
                    //edges of the right frame
                    int xLeftPos = (int)Mathf.Clamp(newVal.x + _loadedFrameHalfSize.x, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    int xRightPos = (int)Mathf.Clamp(newVal.x + _loadedFrameHalfSize.x - dx + 1, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    UnloadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }
            }

            if(dy > 0)  //load upper area, unload bottom area
            {
                int xLeftPos = (int)Mathf.Clamp(newVal.x - _loadedFrameHalfSize.x - 1, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                int xRightPos = (int)Mathf.Clamp(newVal.x + _loadedFrameHalfSize.x + 1, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                {
                    //edge of the bottom frame (area unloaded below main camera)
                    int yTopPos = (int)Mathf.Clamp(newVal.y - _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int yBotPos = (int)Mathf.Clamp(newVal.y - _loadedFrameHalfSize.y - dy, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    UnloadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }

                {
                    //edges of the upper frame (area loaded above main camera)
                    int yTopPos = (int)Mathf.Clamp(newVal.y + _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int yBotPos = (int)Mathf.Clamp(newVal.y + _loadedFrameHalfSize.y - dy, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    LoadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }
            }
            else if(dy < 0) //load bottom area, unload upper area
            {
                int xLeftPos = (int)Mathf.Clamp(newVal.x - _loadedFrameHalfSize.x - 1, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                int xRightPos = (int)Mathf.Clamp(newVal.x + _loadedFrameHalfSize.x + 1, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                {
                    //edge of the bottom frame (area loaded below main camera)
                    int yTopPos = (int)Mathf.Clamp(newVal.y - _loadedFrameHalfSize.y - dy, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int yBotPos = (int)Mathf.Clamp(newVal.y - _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    LoadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }

                {
                    //edge of the upper frame (area unloaded above main camera)
                    int yTopPos = (int)Mathf.Clamp(newVal.y + _loadedFrameHalfSize.y - dy, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int yBotPos = (int)Mathf.Clamp(newVal.y + _loadedFrameHalfSize.y, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    UnloadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }
            }
        }

        public void OnPlayerTranslated(EventArgs args)
        {
            if(args is PlayerTranslatedEA pt)
            {
                {
                    int yTopPos = (int)Mathf.Clamp(pt.OldGridPosition.y + _loadedFrameHalfSize.y + 3, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int yBotPos = (int)Mathf.Clamp(pt.OldGridPosition.y - _loadedFrameHalfSize.y - 3, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int xLeftPos = (int)Mathf.Clamp(pt.OldGridPosition.x - _loadedFrameHalfSize.x - 3, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    int xRightPos = (int)Mathf.Clamp(pt.OldGridPosition.x + _loadedFrameHalfSize.x + 3, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    UnloadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }

                {
                    int yTopPos = (int)Mathf.Clamp(pt.NewGridPostion.y + _loadedFrameHalfSize.y + 3, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int yBotPos = (int)Mathf.Clamp(pt.NewGridPostion.y - _loadedFrameHalfSize.y - 3, _worldInfo.LoadableGridVerticalRange.x, _worldInfo.LoadableGridVerticalRange.y);
                    int xLeftPos = (int)Mathf.Clamp(pt.NewGridPostion.x - _loadedFrameHalfSize.x - 3, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    int xRightPos = (int)Mathf.Clamp(pt.NewGridPostion.x + _loadedFrameHalfSize.x + 3, _worldInfo.LoadableGridHorizontalRange.x, _worldInfo.LoadableGridHorizontalRange.y);
                    LoadArea(xLeftPos, yTopPos, Mathf.Abs(xRightPos - xLeftPos), Mathf.Abs(yTopPos - yBotPos));
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void UnloadArea(int xLeftPos, int yTopPos, int width, int height)
        {
            
            for (int y = yTopPos; y > yTopPos - height; --y)
            {
                for (int x = xLeftPos; x < xLeftPos + width; ++x)
                {
                    _tilemapController.UnloadTile(new Vector2Int(x, y));
                }
            }
        }

        private void LoadArea(int xLeftPos, int yTopPos, int width, int height)
        {
            for (int y = yTopPos; y > yTopPos - height; --y)
            {
                for (int x = xLeftPos; x < xLeftPos + width; ++x)
                {
                    _tilemapController.LoadTile(new Vector2Int(x, y));
                }
            }
        }

        public bool IsTileLoaded(Vector2Int pos)
        {
            if (pos.x >= _cameraGridPosition.Value.x - _loadedFrameHalfSize.x && pos.x <= _cameraGridPosition.Value.x + _loadedFrameHalfSize.x &&
               pos.y >= _cameraGridPosition.Value.y - _loadedFrameHalfSize.y && pos.y <= _cameraGridPosition.Value.y + _loadedFrameHalfSize.y)
                return true;
            return false;
        }

        private void Awake()
        {
            _cameraGridPosition.ValueChanged += OnChangeCameraGridPosition;
        }

        private void OnDestroy()
        {
            _cameraGridPosition.ValueChanged -= OnChangeCameraGridPosition;
        }
    }
}