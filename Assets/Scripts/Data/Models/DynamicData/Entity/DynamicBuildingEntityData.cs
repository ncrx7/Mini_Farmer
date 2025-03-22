

using System;
using System.Collections;
using System.Collections.Generic;
using Data.Models.FixedScriptableData;
using Entities.BuildingEntities.InterfaceHandlers;
using Enums;
using Interfaces;

namespace Data.Models.DynamicData
{
    [Serializable]
    public class DynamicBuildingEntityData
    {
        public int EntityId;
        public EntityType EntityType;
        public FixedBuildingEntityData FixedBuildingEntityData;
        public int CurrentProductInStorage;
        public string ProduceLastProcessTime;
        public List<BuildingProduceProduction> ProductionList;

        public DynamicBuildingEntityData(int entityId, EntityType entityType, FixedBuildingEntityData fixedEntityData, int currentProductInStorage, List<BuildingProduceProduction> productionQueue)
        {
            EntityId = entityId;
            EntityType = entityType;
            FixedBuildingEntityData = fixedEntityData;
            CurrentProductInStorage = currentProductInStorage;
            ProductionList = productionQueue;
        }

        public DateTime GetProduceLastProcessTime()
        {
            return DateTime.TryParse(ProduceLastProcessTime, out DateTime result) ? result : DateTime.MinValue;
        }
    }
}
