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
using UI.PrefabUIs;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.BuildingEntities
{
    public class BuildingEntityManager : EntityManager<DynamicBuildingEntityData>
    {
        [SerializeField] private BuildingEntitySetupper _buildingEntitySetupper;
        [SerializeField] private EntityType _entityType;
        private Queue<IProduction<DynamicBuildingEntityData>> _productionsCommands;
        public bool BuildingIsProducting = false;

        [Header("World Space Building Entity UI References")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _productInStorageText;
        [SerializeField] private TextMeshProUGUI _productTimeText;
        [SerializeField] private TextMeshProUGUI _storageCapacityRateText;
        [SerializeField] private Image _productionProduceImage;
        [SerializeField] private ProductButtonPanelController _productionButtonsPanel;


        private async void Start()
        {
            await UniTask.WaitUntil(() => IsCreateProcessFinished == true);

            Action increaseButtonEvent = () =>
            {
                if (BuildingIsProducting)
                {
                    _productionsCommands.Enqueue(new BuildingProduceProduction(entityData.FixedBuildingEntityData.ProductionTime));

                    entityData.ProductionList = _productionsCommands.Cast<BuildingProduceProduction>().ToList();

                    GameDataManager.Instance.UpdatePlayerDataFile();

                    _storageCapacityRateText.text = GetStorageCapacityRate();
                }
                else
                {
                    entityData.ProductionList.Add(new BuildingProduceProduction(entityData.FixedBuildingEntityData.ProductionTime));
                    StartBuildingProduction().Forget();
                }
            };

            GameEventHandler.OnBuildingEntitySpawnOnScene?.Invoke(this, entityData.CurrentProductInStorage.ToString(),
                                                                GetStorageCapacityRate(),
                                                                entityData.FixedBuildingEntityData.ProductionProcut.StatSprite,
                                                                increaseButtonEvent);

            InitializeBuilding();
        }

        private async void InitializeBuilding()
        {
            await CalculateProductsByElapseTime();
            await StartBuildingProduction();
        }

        private async UniTask StartBuildingProduction()
        {
            if (entityData.ProductionList.Count == 0)
                return;

            BuildingIsProducting = true;

            _productionsCommands = new Queue<IProduction<DynamicBuildingEntityData>>(entityData.ProductionList);

            while (_productionsCommands.Count > 0)
            {
                entityData.ProduceLastProcessTime = DateTime.UtcNow.ToString("O");

                IProduction<DynamicBuildingEntityData> productionCommand = _productionsCommands.Dequeue();

                await productionCommand.StartProduction(entityData, this);

                entityData.ProductionList = _productionsCommands.Cast<BuildingProduceProduction>().ToList();

                GameDataManager.Instance.UpdatePlayerDataFile();

                GameEventHandler.OnProductionEnd?.Invoke(this, entityData.CurrentProductInStorage, 0);
            }

            BuildingIsProducting = false;
        }

        private async UniTask CalculateProductsByElapseTime()
        {
            if (entityData.ProduceLastProcessTime == null || entityData.ProductionList.Count == 0)
                return;

            int secondElapsed = GetElapsedTime();

            for (int i = 0; i <= entityData.ProductionList.Count - 1; i++)
            {
                BuildingProduceProduction production = entityData.ProductionList[i];

                if (secondElapsed - production.CurrentRemainProductionTime > 0)
                {
                    secondElapsed -= production.CurrentRemainProductionTime;

                    entityData.ProductionList.RemoveAt(i);
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

            GameEventHandler.OnCompleteCalculateProductByElapsedTime?.Invoke(this, entityData.CurrentProductInStorage.ToString(), GetStorageCapacityRate());

            await UniTask.DelayFrame(1);
        }

        private int GetElapsedTime()
        {
            TimeSpan timeDifference = DateTime.Now - entityData.GetProduceLastProcessTime();
            return Mathf.Max((int)timeDifference.TotalSeconds, 0); ;
        }

        private string GetStorageCapacityRate()
        {
            return $"{entityData.ProductionList.Count + entityData.CurrentProductInStorage} / {entityData.FixedBuildingEntityData.BuildingStorageMaxCapacity}";
        }

        public int GetProductionQueueCount => _productionsCommands.Count;

        public Slider GetSlider => _slider;
        public TextMeshProUGUI GetCurrentStorageText => _productInStorageText;
        public TextMeshProUGUI GetProductTimeText => _productTimeText;
        public TextMeshProUGUI GetStorageCapacityRateText => _storageCapacityRateText;
        public Image GetProductionProduceImage => _productionProduceImage;
        public ProductButtonPanelController GetProductionButtonsPanel => _productionButtonsPanel;
    }
}
