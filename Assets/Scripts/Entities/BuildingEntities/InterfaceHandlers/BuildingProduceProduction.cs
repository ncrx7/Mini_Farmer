using Interfaces;
using Cysharp.Threading.Tasks;
using Data.Models.DynamicData;
using Data.Controllers;
using System;
using UnityEngine;

namespace Entities.BuildingEntities.InterfaceHandlers
{
    [Serializable]
    public class BuildingProduceProduction : IProduction<DynamicBuildingEntityData>
    {
        public int CurrentProductionTime { get; set; }
        public int productionId;

        public async UniTask StartProduction(DynamicBuildingEntityData dynamicBuildingEntityData)
        {
            int productionTime = dynamicBuildingEntityData.FixedBuildingEntityData.ProductionTime;

            StatType resourceStatType = dynamicBuildingEntityData.FixedBuildingEntityData.ResourceProduct.StatType;
            StatType productStatType = dynamicBuildingEntityData.FixedBuildingEntityData.ProductionProcut.StatType;

            while (productionTime > 0)
            {
                productionTime--;
                CurrentProductionTime++;
                Debug.Log("producing : " + dynamicBuildingEntityData.FixedBuildingEntityData.ProductionProcut.StatName + productionTime);

                if(productionTime == 0)
                {
                    GameDataManager.Instance.GetDynamicStatData(resourceStatType).Amount -= dynamicBuildingEntityData.FixedBuildingEntityData.ResourceAmount;
                    GameDataManager.Instance.GetDynamicStatData(productStatType).Amount += dynamicBuildingEntityData.FixedBuildingEntityData.ProductAmount;
                }

                await UniTask.Delay(1000);
            }
        }

        public BuildingProduceProduction(int id)
        {
            productionId = id;
        }
    }
}
