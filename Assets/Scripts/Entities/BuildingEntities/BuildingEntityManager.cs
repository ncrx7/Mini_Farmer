using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Data.Models.DynamicData;
using Entities.BuildingEntities.InterfaceHandlers;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.BuildingEntities
{
    public class BuildingEntityManager : EntityManager<DynamicBuildingEntityData>
    {
        [SerializeField] private BuildingEntitySetupper _buildingEntitySetupper;
        [SerializeField] private EntityType _entityType;

        [Header("World Space Building Entity UI References")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _productInStorageText;
        [SerializeField] private TextMeshProUGUI _productTimeText;
        [SerializeField] private TextMeshProUGUI _productQueueAmountText;
        private Queue<IProduction<DynamicBuildingEntityData>> _productionsCommands;

        private async void Start()
        {
            await UniTask.WaitUntil(() => IsCreateProcessFinished == true);

            GameEventHandler.OnBuildingEntitySpawnOnScene?.Invoke(_productInStorageText, _productTimeText, _productQueueAmountText, entityData);

            _productionsCommands = new Queue<IProduction<DynamicBuildingEntityData>>(entityData.ProduceQueue);
            CheckElapsedTimeProduce();

            StartBuildingProduction();
        }

        private async void StartBuildingProduction()
        {
            while (_productionsCommands.Count > 0)
            {
                entityData.ProduceLastProcessTime = DateTime.UtcNow.ToString("O");

                IProduction<DynamicBuildingEntityData> productionCommand = _productionsCommands.Dequeue();

                Debug.Log("product produce start");
                await productionCommand.StartProduction(entityData, this);
                Debug.Log("product produce end");

                GameDataManager.Instance.UpdatePlayerDataFile();
                GameEventHandler.OnProductionEnd?.Invoke(this, entityData.CurrentProductInStorage, 0);
                //GameEventHandler
            }
        }

        private void CheckElapsedTimeProduce()
        {
            if (entityData.ProduceLastProcessTime == null && entityData.ProduceQueue.Count == 0)
            {
                Debug.Log("Last process time is null");
                return;
            }

            TimeSpan timeDifference = DateTime.Now - entityData.GetProduceLastProcessTime();
            int secondElapsed = Mathf.Max((int)timeDifference.TotalSeconds, 0);
            Debug.Log("Last process time is not null and total elapsed second is : " + secondElapsed);

            if (secondElapsed <= entityData.ProduceQueue[0].CurrentRemainProductionTime)
                return;

            int productionFinishedAmount = secondElapsed / entityData.FixedBuildingEntityData.ProductionTime;

            int iterationNumber = productionFinishedAmount <= _productionsCommands.Count ? productionFinishedAmount : _productionsCommands.Count;

            for (int i = 0; i < iterationNumber; i++)
            {
                _productionsCommands.Dequeue();
                GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount -= entityData.FixedBuildingEntityData.ResourceAmount;
                entityData.CurrentProductInStorage += entityData.FixedBuildingEntityData.ProductAmount;
            }

            _productInStorageText.text = entityData.CurrentProductInStorage.ToString();
            _productTimeText.text = "";
            _productQueueAmountText.text = $"{_productionsCommands.Count} / {entityData.FixedBuildingEntityData.BuildingStorageMaxCapacity}";
            _slider.value = 1;
        }

        public int GetProductionQueueCount => _productionsCommands.Count;

        public Slider GetSlider => _slider;
        public TextMeshProUGUI GetCurrentStorageText => _productInStorageText;
        public TextMeshProUGUI GetProductTimeText => _productTimeText;
        public TextMeshProUGUI GetProductQueueAmountText => _productQueueAmountText;
    }
}
