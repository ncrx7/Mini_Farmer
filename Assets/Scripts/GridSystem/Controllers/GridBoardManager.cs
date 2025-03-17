using System.Collections;
using System.Collections.Generic;
using UnityUtils.BaseClasses;
using NodeGridSystem.Models;
using UnityEngine;
using Cysharp;
using Cysharp.Threading.Tasks;
using Entities.PlaneEntities;

namespace NodeGridSystem.Controllers
{
    public class GridBoardManager : SingletonBehavior<GridBoardManager>
    {
        [SerializeField] private EntityLoaderFromJson _entityLoader;

        [Header("Grid Settings")]
        [SerializeField] private int _width = 6;
        [SerializeField] private int _height = 6;
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private Vector3 _originPosition = Vector3.zero;
        [SerializeField] private bool _debug = true;

        private GridSystem2D<GridObject<GrassAreaManager>> _gridSystem;

        private void Start()
        {
            InitializeBoard();
        }

        private async void InitializeBoard()
        {
            _gridSystem = GridSystem2D<GridObject<GrassAreaManager>>.HorizontalGrid(_width, _height, _cellSize, _originPosition, _debug);

            //MiniEventSystem.ActivateLoadingUI?.Invoke();
            //GameManager.Instance.IsGamePaused = true;

            await InitBoard();

            await UniTask.Delay(1000);

            //GameManager.Instance.IsGamePaused = false;
            //MiniEventSystem.DeactivateLoadingUI?.Invoke();
        }

        private async UniTask InitBoard()
        {
            _entityLoader.LoadEntitiesFromJson(_gridSystem);

            await UniTask.DelayFrame(1);
        }

        public float GetCellSize => _cellSize;
        public int GetWidth => _width;
        public int GetHeight => _height;
        public GridSystem2D<GridObject<GrassAreaManager>> GetNodeGridSystem2D => _gridSystem;
        public EntityLoaderFromJson GetEntityLoader => _entityLoader;

    }
}