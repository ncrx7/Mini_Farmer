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

            InitializeBuilding();
        }

        private async void InitializeBuilding()
        {
            await CalculateProductsByElapseTime();
            await StartBuildingProduction();
        }

        private async UniTask StartBuildingProduction()
        {
            if (entityData.ProduceList.Count == 0)
                return;

            _productionsCommands = new Queue<IProduction<DynamicBuildingEntityData>>(entityData.ProduceList);

            while (_productionsCommands.Count > 0)
            {
                entityData.ProduceLastProcessTime = DateTime.UtcNow.ToString("O");

                IProduction<DynamicBuildingEntityData> productionCommand = _productionsCommands.Dequeue();

                await productionCommand.StartProduction(entityData, this);

                entityData.ProduceList = _productionsCommands.Cast<BuildingProduceProduction>().ToList();

                GameDataManager.Instance.UpdatePlayerDataFile();

                GameEventHandler.OnProductionEnd?.Invoke(this, entityData.CurrentProductInStorage, 0);
            }
        }

        private async UniTask CalculateProductsByElapseTime()
        {
            if (entityData.ProduceLastProcessTime == null || entityData.ProduceList.Count == 0)
                return;

            int secondElapsed = GetElapsedTime();

            for (int i = 0; i <= entityData.ProduceList.Count - 1; i++)
            {
                BuildingProduceProduction production = entityData.ProduceList[i];

                if (secondElapsed - production.CurrentRemainProductionTime > 0)
                {
                    secondElapsed -= production.CurrentRemainProductionTime;

                    entityData.ProduceList.RemoveAt(i);
                    i--;

                    GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount -= entityData.FixedBuildingEntityData.ResourceAmount;
                    entityData.CurrentProductInStorage += entityData.FixedBuildingEntityData.ProductAmount;
                }
                else
                {
                    production.CurrentRemainProductionTime -= secondElapsed;
                    break;
                }
            }

            GameDataManager.Instance.UpdatePlayerDataFile();

            _productInStorageText.text = entityData.CurrentProductInStorage.ToString();
            _productTimeText.text = "";
            _productQueueAmountText.text = $"{entityData.ProduceList.Count} / {entityData.FixedBuildingEntityData.BuildingStorageMaxCapacity}";
            _slider.value = 1;

            await UniTask.DelayFrame(1);
        }

        private int GetElapsedTime()
        {
            TimeSpan timeDifference = DateTime.Now - entityData.GetProduceLastProcessTime();
            return Mathf.Max((int)timeDifference.TotalSeconds, 0);;
        }

        public int GetProductionQueueCount => _productionsCommands.Count;

        public Slider GetSlider => _slider;
        public TextMeshProUGUI GetCurrentStorageText => _productInStorageText;
        public TextMeshProUGUI GetProductTimeText => _productTimeText;
        public TextMeshProUGUI GetProductQueueAmountText => _productQueueAmountText;
    }
}
