using System.Collections;
using System.Collections.Generic;
using Data.Models.DynamicData;
using Enums;
using UnityEngine;

namespace Entities.PlaneEntities
{
    public class GrassAreaManager : EntityManager<GrassAreaData>
    {
        [SerializeField] private EntityType _entityType;
    }
}
