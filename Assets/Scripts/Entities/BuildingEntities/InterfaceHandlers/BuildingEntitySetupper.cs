using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Data.Models.DynamicData;
using Data.Models.FixedScriptableData;
using Entities.PlaneEntities;
using Interfaces;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;

namespace Entities.BuildingEntities.InterfaceHandlers
{
    public class BuildingEntitySetupper : MonoBehaviour, IEntitySetup
    {
        public void SetupEntity(GridSystem2D<GridObject<GrassAreaManager>> gridSystem, FixedBaseEntityData fixedEntityData)
        {
            FixedBuildingEntityData fixedBuildingEntityData = fixedEntityData as FixedBuildingEntityData;

            bool shouldBreakLoops = false;

            for (int y = 0; y < GridBoardManager.Instance.GetHeight; y++)
            {
                if (shouldBreakLoops) break;

                for (int x = 0; x < GridBoardManager.Instance.GetWidth; x++)
                {
                    if (gridSystem.GetValue(x, y) == null)
                        return;

                    GrassAreaData grassAreaData = gridSystem.GetValue(x, y).GetValue().entityData;

                    if (grassAreaData == null)
                        return;

                    if (grassAreaData.IsEmpty && CheckMoneyEnough(fixedEntityData))
                    {
                        PayPrice(fixedEntityData);

                        GameEventHandler.OnCreateEntity?.Invoke(fixedEntityData, GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount);

                        Vector3 targetPosition = gridSystem.GetWorldPositionCenter(x, y);
                        targetPosition.y += fixedBuildingEntityData.SpawnYOffset;
                        
                        EntityManager<DynamicBuildingEntityData> buildingEntity = Instantiate(fixedEntityData.EntityPrefab, targetPosition, Quaternion.Euler(fixedBuildingEntityData.SpawnRotation), GridBoardManager.Instance.GetEntityLoader.transform)
                         as EntityManager<DynamicBuildingEntityData>;

                        //buildingEntity.entityData.FixedBuildingEntityData = fixedBuildingEntityData;

                        buildingEntity.gameObject.DoElasticStretch(new Vector3(0.5f, 2f, 0.5f), 1.5f, () => Debug.Log("entity spawned"));

                        //buildingEntity.GetComponent<BuildingEntityManager>()

                        grassAreaData.IsEmpty = false;

                        SaveBuildEntityData(fixedBuildingEntityData, grassAreaData, buildingEntity);
                        

                        shouldBreakLoops = true;
                        break;
                    }
                }
            }
        }

        private void SaveBuildEntityData(FixedBuildingEntityData fixedBuildingEntityData, GrassAreaData grassAreaData, EntityManager<DynamicBuildingEntityData> buildingEntity)
        {
            DynamicBuildingEntityData dynamicBuildingEntityData = new(0, fixedBuildingEntityData.EntityType, fixedBuildingEntityData, 0, new()); 
            dynamicBuildingEntityData.ProduceQueue.Add(new BuildingProduceProduction());
            dynamicBuildingEntityData.ProduceQueue.Add(new BuildingProduceProduction());

            buildingEntity.entityData = dynamicBuildingEntityData;

            GameDataManager.Instance.GetGameDataReference.BuildingEntityDatas.Add(dynamicBuildingEntityData);

            grassAreaData.DynamicBuildingEntityData = dynamicBuildingEntityData;

            GameDataManager.Instance.UpdatePlayerDataFile();  

            buildingEntity.IsCreateProcessFinished = true;
        }

        private void PayPrice(FixedBaseEntityData fixedEntityData)
        {
            GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount -= fixedEntityData.MarketPrice;
        }

        private bool CheckMoneyEnough(FixedBaseEntityData fixedEntityData)
        {
            return GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount >= fixedEntityData.MarketPrice;
        }
    }
}
