

using System;
using Data.Models.FixedScriptableData;

namespace Data.Models.DynamicData
{
    [Serializable]
    public class DynamicBuildingEntityData
    {
        public int EntityId;
        public EntityType EntityType;
        public FixedBuildingEntityData FixedBuildingEntityData;
        public int CurrentProductInStorage;

        public DynamicBuildingEntityData(int entityId, EntityType entityType, FixedBuildingEntityData fixedEntityData, int currentProductInStorage)
        {
            EntityId = entityId;
            EntityType = entityType;
            FixedBuildingEntityData = fixedEntityData;
            CurrentProductInStorage = currentProductInStorage;
        }
    }
}
