using System.Collections;
using System.Collections.Generic;
using Data.Models.DynamicData;
using UnityEngine;

namespace Entities.BuildingEntities
{
    public class BuildingEntityManager : EntityManager<DynamicBuildingEntityData>
    {
        [SerializeField] private EntityType _entityType;
    }
}
