using System.Collections;
using System.Collections.Generic;
using Data.Models.FixedScriptableData;
using Entities.PlaneEntities;
using NodeGridSystem.Models;
using UnityEngine;

namespace Interfaces
{
    public interface IEntitySetup
    {
        public void SetupEntity(GridSystem2D<GridObject<GrassAreaManager>> gridSystem2D, FixedBaseEntityData fixedEntityData);
    }
}
