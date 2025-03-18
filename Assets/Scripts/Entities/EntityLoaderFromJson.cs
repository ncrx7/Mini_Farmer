using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Data.Models.DynamicData;
using DG.Tweening;
using Entities.PlaneEntities;
using NodeGridSystem.Models;
using UnityEngine;

public class EntityLoaderFromJson : MonoBehaviour
{
    [SerializeField] private GrassAreaManager _grassAreaPrefab;
    public bool AreEntitiesLoadFinished = false;


    [Header("Entity Load Animation Settings")]
    [SerializeField] private Vector3 _scaleFactor;
    [SerializeField] private float _animationDuration;

    public async void LoadEntitiesFromJson(GridSystem2D<GridObject<GrassAreaManager>> gridSystem2D)
    {
        GameEventHandler.OnStartEntitesLoad?.Invoke();

        await LoadGrassAreaEntities(gridSystem2D);

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

            grassArea.grassAreaData = grassAreaData;
            
            grassArea.gameObject.DoElasticStretch(_scaleFactor, _animationDuration, () => LoadBuildingEntityOnGrass(grassArea));

            await UniTask.Delay(500);
        }

        await UniTask.Delay(100);
    }

    private void LoadBuildingEntityOnGrass(GrassAreaManager grassAreaManager)
    {
        if (!grassAreaManager.grassAreaData.IsEmpty)
            return;

        GameObject buildingEntity = Instantiate(grassAreaManager.grassAreaData.DynamicBuildingEntityData.FixedEntityData.EntityPrefab, grassAreaManager.transform.position, Quaternion.identity, grassAreaManager.transform);
        buildingEntity.DoElasticStretch(_scaleFactor, _animationDuration, () => grassAreaManager.grassAreaData.IsEmpty = false);
    }
}
