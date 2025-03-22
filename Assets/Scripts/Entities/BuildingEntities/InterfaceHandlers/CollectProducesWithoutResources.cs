using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Enums;
using Interfaces;
using UnityEngine;

namespace Entities.BuildingEntities.InterfaceHandlers
{
    public class CollectProducesWithoutResources : ICollect
    {
        public void TryCollectProduces(EntityManager entityManager, Action callBack)
        {
            BuildingEntityManager buildingEntityManager = entityManager as BuildingEntityManager;

            if (buildingEntityManager.entityData.CurrentProductInStorage <= 0)
                return;

            GameDataManager.Instance.GetDynamicStatData(buildingEntityManager.entityData.FixedBuildingEntityData.ProductionProcut.StatType).Amount += buildingEntityManager.entityData.CurrentProductInStorage;

            MakeProductionQueueFull(buildingEntityManager);

            buildingEntityManager.entityData.CurrentProductInStorage = 0;

            GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount += buildingEntityManager.entityData.FixedBuildingEntityData.EarnMoneyAmount;
            GameDataManager.Instance.GetDynamicStatData(StatType.Xp).Amount += buildingEntityManager.entityData.FixedBuildingEntityData.EarnXpAmount;

            GameDataManager.Instance.UpdatePlayerDataFile();

            GameEventHandler.PlayVfx?.Invoke(entityManager.transform.position, VfxType.CollectProduces);
            GameEventHandler.PlaySoundClip(SoundType.CollectProduces);
        }

        private void MakeProductionQueueFull(BuildingEntityManager buildingEntityManager)
        {
            if (!buildingEntityManager.BuildingIsProducting)
            {
                AddCertainProduction(0, buildingEntityManager, buildingEntityManager.entityData.FixedBuildingEntityData.BuildingStorageMaxCapacity);

                buildingEntityManager.StartBuildingProduction().Forget();
            }
            else
            {
                AddCertainProduction(1, buildingEntityManager, buildingEntityManager.entityData.CurrentProductInStorage);
            }
        }

        private void AddCertainProduction(int processId, BuildingEntityManager buildingEntityManager, int iterationNumber) //0: TO production List, 1: TO production Queue
        {
            for (int i = 0; i < iterationNumber; i++)
            {
                if (processId == 0)
                    buildingEntityManager.entityData.ProductionList.Add(new BuildingProduceProduction(buildingEntityManager.entityData.FixedBuildingEntityData.ProductionTime));
                else
                    buildingEntityManager.AddProductionToQueue();
            }

            if (processId == 0)
                return;

            buildingEntityManager.SaveProductionQueue();
        }
    }
}
