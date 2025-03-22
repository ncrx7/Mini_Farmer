using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Data.Models.DynamicData;
using Data.Models.FixedScriptableData;
using DG.Tweening;
using Entities;
using Entities.PlaneEntities;
using Enums;
using GridSystem.Models;
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

            EntityManager<GrassAreaData> grassArea = Instantiate(_grassAreaPrefab, grassWorldPosition, Quaternion.identity, transform);

            GridObject<GrassAreaManager> gridObject = new(gridSystem2D, grassAreaData.XGridPosition, grassAreaData.YGridPosition);
            gridObject.SetValue(grassArea as GrassAreaManager);
            gridSystem2D.SetValue(grassAreaData.XGridPosition, grassAreaData.YGridPosition, gridObject);

            grassArea.entityData = grassAreaData;
            
            grassArea.gameObject.DoElasticStretch(_scaleFactor, _animationDuration, () => LoadBuildingEntityOnGrass(grassArea as GrassAreaManager));

            await UniTask.Delay(100);
        }

        await UniTask.Delay(100);
    }

    private void LoadBuildingEntityOnGrass(GrassAreaManager grassAreaManager)
    {
        if (grassAreaManager.entityData.IsEmpty)
            return;
        
        DynamicBuildingEntityData dynamicBuildingEntityData = grassAreaManager.entityData.DynamicBuildingEntityData;

        Vector3 targetPos = grassAreaManager.transform.position;
        targetPos.y += dynamicBuildingEntityData.FixedBuildingEntityData.SpawnYOffset;

        EntityManager<DynamicBuildingEntityData> buildingEntity = Instantiate(dynamicBuildingEntityData.FixedBuildingEntityData.EntityPrefab, targetPos,
                Quaternion.Euler(dynamicBuildingEntityData.FixedBuildingEntityData.SpawnRotation), grassAreaManager.transform) as EntityManager<DynamicBuildingEntityData>;

        buildingEntity.entityData = dynamicBuildingEntityData;

        buildingEntity.IsCreateProcessFinished = true;

        buildingEntity.gameObject.DoElasticStretch(_scaleFactor, _animationDuration, () => grassAreaManager.entityData.IsEmpty = false);

        GameEventHandler.PlayVfx?.Invoke(targetPos, VfxType.SpawnBuilding);
        GameEventHandler.PlaySoundClip(SoundType.SpawnBuilding);
    }
}
