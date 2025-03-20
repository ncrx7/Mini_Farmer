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
        public int CurrentRemainProductionTime;

        public BuildingProduceProduction(int remainTime)
        {
            CurrentRemainProductionTime = remainTime;
        }

        public async UniTask StartProduction(DynamicBuildingEntityData dynamicBuildingEntityData, EntityManager<DynamicBuildingEntityData> entityManager)
        {
            StatType resourceStatType = dynamicBuildingEntityData.FixedBuildingEntityData.ResourceProduct.StatType;
            StatType productStatType = dynamicBuildingEntityData.FixedBuildingEntityData.ProductionProcut.StatType;

            GameDataManager.Instance.GetDynamicStatData(resourceStatType).Amount -= dynamicBuildingEntityData.FixedBuildingEntityData.ResourceAmount;

            string remainCommandAmount = $"{((BuildingEntityManager)entityManager).GetProductionQueueCount} / {dynamicBuildingEntityData.FixedBuildingEntityData.BuildingStorageMaxCapacity}";
            
            GameEventHandler.OnProductionStart?.Invoke(entityManager as BuildingEntityManager, CurrentRemainProductionTime,
                                                         resourceStatType, remainCommandAmount);

            while (CurrentRemainProductionTime > 0)
            {
                CurrentRemainProductionTime--;
                
                float sliderValue = (float)(dynamicBuildingEntityData.FixedBuildingEntityData.ProductionTime - CurrentRemainProductionTime) / dynamicBuildingEntityData.FixedBuildingEntityData.ProductionTime;
                GameEventHandler.OnProductionContinue?.Invoke(entityManager as BuildingEntityManager, CurrentRemainProductionTime, sliderValue);

                dynamicBuildingEntityData.ProduceLastProcessTime = DateTime.UtcNow.ToString("O");
                
                GameDataManager.Instance.UpdatePlayerDataFile();

                if(CurrentRemainProductionTime == 0)
                {
                    dynamicBuildingEntityData.CurrentProductInStorage += dynamicBuildingEntityData.FixedBuildingEntityData.ProductAmount;
                }

                await UniTask.Delay(1000);
            }
        }
    }
}
