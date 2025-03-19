using System.Collections;
using System.Collections.Generic;
using Data.Models.DynamicData;
using TMPro;
using UnityEngine;

namespace Entities.BuildingEntities
{
    public class BuildingEntityManager : EntityManager<DynamicBuildingEntityData>
    {
        [SerializeField] private EntityType _entityType;

        [Header("World Space Building Entity UI References")]
        [SerializeField] private TextMeshProUGUI _productInStorageText;
        [SerializeField] private TextMeshProUGUI _productTimeText;
        [SerializeField] private TextMeshProUGUI _productQueueAmountText;

        private void Start()
        {
            GameEventHandler.OnBuildingEntitySpawnOnScene?.Invoke(_productInStorageText, _productTimeText, _productQueueAmountText, entityData);
        }
    }
}
