using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using DG.Tweening;
using Entities.PlaneEntities;
using NodeGridSystem.Models;
using UnityEngine;

public class EntityLoaderFromJson : MonoBehaviour
{
    [SerializeField] private GrassAreaManager _grassAreaPrefab;
    private List<GrassAreaManager> _grassAreaManagers = new();
    public bool AreEntitiesLoadFinished = false;

    [Header("Entity Load Animation Settings")]
    [SerializeField] private Vector3 _scaleFactor;
    [SerializeField] private float _animationDuration;

    public async void LoadEntitiesFromJson(GridSystem2D<GridObject<GrassAreaManager>> gridSystem2D)
    {
        GameEventHandler.OnStartEntitesLoad?.Invoke();

        await LoadGrassAreaEntities(gridSystem2D);
        await LoadBuildingEntities();

        AreEntitiesLoadFinished = true;

        GameEventHandler.OnCompleteEntitiesLoad?.Invoke();
    }

    private async UniTask LoadGrassAreaEntities(GridSystem2D<GridObject<GrassAreaManager>> gridSystem2D)
    {
        foreach (var grassAreaData in GameDataManager.Instance.GetGameDataReference.GrasAreaDatas)
        {
            Vector3 grassWorldPosition = gridSystem2D.GetWorldPositionCenter(grassAreaData.XGridPosition, grassAreaData.YGridPosition);

            GrassAreaManager grassArea = Instantiate(_grassAreaPrefab, grassWorldPosition, Quaternion.identity, transform);

            GridObject<GrassAreaManager> gridObject = new(gridSystem2D, grassAreaData.XGridPosition, grassAreaData.YGridPosition);
            gridObject.SetValue(grassArea);
            gridSystem2D.SetValue(grassAreaData.XGridPosition, grassAreaData.YGridPosition, gridObject);

            grassArea.gameObject.DoElasticStretch(_scaleFactor, _animationDuration, () => Debug.Log("callback"));

            _grassAreaManagers.Add(grassArea);

            grassArea.grassAreaData = grassAreaData;

            await UniTask.Delay(500);
        }

        await UniTask.Delay(100);
    }

    private async UniTask LoadBuildingEntities()
    {
        foreach (var grassAreaManager in _grassAreaManagers)
        {
            if (grassAreaManager.grassAreaData.IsEmpty)
                return;

            //TODO: INSTANTIATE BUILDING ENTITY
        }

        await UniTask.Delay(1000);
    }
}
