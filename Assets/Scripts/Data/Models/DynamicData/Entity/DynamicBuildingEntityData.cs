

using System;
using Data.Models.FixedScriptableData;

namespace Data.Models.DynamicData
{
    [Serializable]
    public class DynamicBuildingEntityData
    {
        public int EntityId;
        public EntityType EntityType;
        public FixedEntityData FixedEntityData;

        public DynamicBuildingEntityData(int entityId, EntityType entityType, FixedEntityData fixedEntityData)
        {
            EntityId = entityId;
            EntityType = entityType;
            FixedEntityData = fixedEntityData;
        }
    }
}
