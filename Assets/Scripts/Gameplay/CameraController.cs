﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1f)] private float _rigidity = 2f;
        [SerializeField] private Transform _target = null;
        private Camera _camera = null;
        private Grid _worldGrid = null;
        [SerializeField] private Vector2IntReference _verticalWorldBorders = null;
        [SerializeField] private Vector2IntReference _horizontalWorldBorders = null;
        private float _topBorder = 0f;
        private float _bottomBorder = 0f;
        private float _leftBorder = 0f;
        private float _rightBorder = 0f;
        

        public void OnWorldLoaded()
        {
            _worldGrid = FindObjectOfType<Grid>();
            _leftBorder = _horizontalWorldBorders.Value.x * _worldGrid.cellSize.x + _camera.orthographicSize * Screen.width/Screen.height;
            _rightBorder = _horizontalWorldBorders.Value.y * _worldGrid.cellSize.x - _camera.orthographicSize * Screen.width / Screen.height;
            _topBorder = _verticalWorldBorders.Value.x * _worldGrid.cellSize.y - _camera.orthographicSize;
            _bottomBorder = _verticalWorldBorders.Value.y * _worldGrid.cellSize.y + _camera.orthographicSize;
        }

        

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        void FixedUpdate()
        {
            Vector3 vec = Vector3.Lerp(transform.position, _target.position, _rigidity);
            float x = Mathf.Clamp(vec.x, _leftBorder, _rightBorder);
            float y = Mathf.Clamp(vec.y, _bottomBorder, _topBorder);
            transform.position = new Vector3(x, y, _camera.transform.position.z);
        }
    }


}