using System;
using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Interfaces;
using UnityEngine;

namespace Entities.BuildingEntities.InterfaceHandlers
{
    public class CollectProducesWithResources : ICollect
    {
        public void TryCollectProduces(EntityManager entityManager, Action callBack)
        {
            BuildingEntityManager buildingEntityManager = entityManager as BuildingEntityManager;

            if (buildingEntityManager.GetProductionButtonsPanel.gameObject.activeSelf)
            {
                GameDataManager.Instance.GetDynamicStatData(buildingEntityManager.entityData.FixedBuildingEntityData.ProductionProcut.StatType).Amount += buildingEntityManager.entityData.CurrentProductInStorage;
                buildingEntityManager.entityData.CurrentProductInStorage = 0;
                GameDataManager.Instance.UpdatePlayerDataFile();
            }
        }
    }
}
