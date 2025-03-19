using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Models.DynamicData;
using Interfaces;
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
        private Queue<IProduction<DynamicBuildingEntityData>> _productionsCommands;

        private void Start()
        {
            GameEventHandler.OnBuildingEntitySpawnOnScene?.Invoke(_productInStorageText, _productTimeText, _productQueueAmountText, entityData);

            _productionsCommands = new Queue<IProduction<DynamicBuildingEntityData>>(entityData.ProduceQueue);

            StartBuildingProduction();
        }

        private async void StartBuildingProduction()
        {
            while ( _productionsCommands.Count > 0)
            {
                IProduction<DynamicBuildingEntityData> productionCommand = _productionsCommands.Dequeue();

                Debug.Log("product produce start");
                await productionCommand.StartProduction(entityData);
                Debug.Log("product produce end");
            }
        }
    }
}
