

using System;
using System.Collections;
using System.Collections.Generic;
using Data.Models.FixedScriptableData;
using Entities.BuildingEntities.InterfaceHandlers;
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
        public List<BuildingProduceProduction> ProduceList;

        public DynamicBuildingEntityData(int entityId, EntityType entityType, FixedBuildingEntityData fixedEntityData, int currentProductInStorage, List<BuildingProduceProduction> produceQueue)
        {
            EntityId = entityId;
            EntityType = entityType;
            FixedBuildingEntityData = fixedEntityData;
            CurrentProductInStorage = currentProductInStorage;
            ProduceList = produceQueue;
        }

        public DateTime GetProduceLastProcessTime()
        {
            return DateTime.TryParse(ProduceLastProcessTime, out DateTime result) ? result : DateTime.MinValue;
        }
    }
}
