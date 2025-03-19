using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Data.Models.FixedScriptableData
{
    [CreateAssetMenu(fileName = "BuildingEntityData", menuName = "Scriptable Objects/Entities/BuildingEntityData")]
    public class FixedBuildingEntityData : FixedBaseEntityData
    {
        [Header("Building Properties")]
        public Vector3 SpawnRotation;
        public float SpawnYOffset;

        [Header("Production Properties")]
        public int BuildingStorageMaxCapacity;
        public int ProductionTime;
        public FixedStatData ResourceProduct;
        public FixedStatData ProductionProcut;
        public int ResourceAmount;
        public int ProductAmount;
    }
}
