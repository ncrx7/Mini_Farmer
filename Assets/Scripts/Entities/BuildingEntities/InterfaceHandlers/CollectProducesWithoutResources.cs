using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Interfaces;
using UnityEngine;

namespace Entities.BuildingEntities.InterfaceHandlers
{
    public class CollectProducesWithoutResources : ICollect
    {
        public void TryCollectProduces(EntityManager entityManager, Action callBack)
        {
            BuildingEntityManager buildingEntityManager = entityManager as BuildingEntityManager;

            GameDataManager.Instance.GetDynamicStatData(buildingEntityManager.entityData.FixedBuildingEntityData.ProductionProcut.StatType).Amount += buildingEntityManager.entityData.CurrentProductInStorage;

            MakeProductionQueueFull(buildingEntityManager);

            buildingEntityManager.entityData.CurrentProductInStorage = 0;

            GameDataManager.Instance.UpdatePlayerDataFile();
        }

        private void MakeProductionQueueFull(BuildingEntityManager buildingEntityManager)
        {
            if (!buildingEntityManager.BuildingIsProducting)
            {
                AddCertainProduction(buildingEntityManager, buildingEntityManager.entityData.FixedBuildingEntityData.BuildingStorageMaxCapacity);

                buildingEntityManager.StartBuildingProduction().Forget();
            }
            else
            {
                AddCertainProduction(buildingEntityManager, buildingEntityManager.entityData.CurrentProductInStorage);

                buildingEntityManager.SaveProductionQueue();
            }
        }

        private void AddCertainProduction(BuildingEntityManager buildingEntityManager, int iterationNumber)
        {
            for (int i = 0; i < iterationNumber; i++)
            {
                buildingEntityManager.AddProductionToQueue();
            }

            buildingEntityManager.SaveProductionQueue();
        }
    }
}
