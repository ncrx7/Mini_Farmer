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
        [SerializeField] private GameObject _spawnVfx;
        [SerializeField] private BuildingEntitySetupper _buildingEntitySetupper;
        [SerializeField] private EntityType _entityType;
        private Queue<IProduction<DynamicBuildingEntityData>> _productionsCommands;
        public bool BuildingIsProducting = false;

        [Header("World Space Building Entity UI References")]
        [SerializeField] private ProductSliderController _productSliderPanel;
        [SerializeField] private ProductButtonPanelController _productionButtonsPanel;


        private async void Start()
        {
            await UniTask.WaitUntil(() => IsCreateProcessFinished == true);

            Action increaseButtonEvent = HandleIncreaseProduction;

            Action reduceButtonEvent = HandleReduceProduction;

            GameEventHandler.OnBuildingEntitySpawnOnScene?.Invoke(this, entityData.CurrentProductInStorage.ToString(),
                                                                GetStorageCapacityRate(0),
                                                                entityData.FixedBuildingEntityData.ProductionProcut.StatSprite,
                                                                increaseButtonEvent,
                                                                reduceButtonEvent);

            InitializeBuildingProduction();
        }

        private async void InitializeBuildingProduction()
        {
            await CalculateProductsByElapseTime();
            await StartBuildingProduction();
        }

        public async UniTask StartBuildingProduction()
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

                    //if (entityData.FixedBuildingEntityData.ResourceProduct != null)
                        //GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount -= entityData.FixedBuildingEntityData.ResourceAmount;

                    entityData.CurrentProductInStorage += entityData.FixedBuildingEntityData.ProductAmount;
                }
                else
                {
                    production.CurrentRemainProductionTime -= secondElapsed;
                    break;
                }
            }

            GameDataManager.Instance.UpdatePlayerDataFile();

            GameEventHandler.OnCompleteCalculateProductByElapsedTime?.Invoke(this, entityData.CurrentProductInStorage.ToString(), GetStorageCapacityRate(0));

            await UniTask.DelayFrame(1);
        }

        private int GetElapsedTime()
        {
            TimeSpan timeDifference = DateTime.Now - entityData.GetProduceLastProcessTime();
            return Mathf.Max((int)timeDifference.TotalSeconds, 0); ;
        }

        private void HandleIncreaseProduction()
        {
            if (GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount -
                                                            entityData.FixedBuildingEntityData.ResourceAmount < 0)
                    return;

            if (BuildingIsProducting)
            {
                _productionsCommands.Enqueue(new BuildingProduceProduction(entityData.FixedBuildingEntityData.ProductionTime));

                entityData.ProductionList = _productionsCommands.Cast<BuildingProduceProduction>().ToList();

                GameDataManager.Instance.UpdatePlayerDataFile();

                _productSliderPanel.GetStorageCapacityRateText.text = GetStorageCapacityRate(1);
            }
            else
            {
                entityData.ProductionList.Add(new BuildingProduceProduction(entityData.FixedBuildingEntityData.ProductionTime));

                StartBuildingProduction().Forget();
            }

            GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount -= entityData.FixedBuildingEntityData.ResourceAmount;

            GameEventHandler.OnClickIncreaseButton?.Invoke(GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount.ToString(),
                                                            entityData.FixedBuildingEntityData.ResourceProduct.StatType);
        }

        private void HandleReduceProduction()
        {
            if (BuildingIsProducting)
            {
                if(_productionsCommands.Count == 0)
                    return;

                _productionsCommands.Dequeue();

                entityData.ProductionList = _productionsCommands.Cast<BuildingProduceProduction>().ToList();

                GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount += entityData.FixedBuildingEntityData.ResourceAmount;

                GameDataManager.Instance.UpdatePlayerDataFile();

                _productSliderPanel.GetStorageCapacityRateText.text = GetStorageCapacityRate(1);

                GameEventHandler.OnClickReduceButton?.Invoke(GameDataManager.Instance.GetDynamicStatData(entityData.FixedBuildingEntityData.ResourceProduct.StatType).Amount.ToString(),
                                                            entityData.FixedBuildingEntityData.ResourceProduct.StatType);
            }
        }

        public string GetStorageCapacityRate(int inProductionCount)
        {
            return $"{entityData.ProductionList.Count + entityData.CurrentProductInStorage + inProductionCount} / {entityData.FixedBuildingEntityData.BuildingStorageMaxCapacity}";
        }

        public void AddProductionToQueue()
        {
            if (_productionsCommands == null)
                return;

            _productionsCommands.Enqueue(new BuildingProduceProduction(entityData.FixedBuildingEntityData.ProductionTime));
        }

        public void SaveProductionQueue()
        {
            if (_productionsCommands == null)
                return;

            entityData.ProductionList = _productionsCommands.Cast<BuildingProduceProduction>().ToList();

            GameDataManager.Instance.UpdatePlayerDataFile();
        }

        public int GetProductionQueueCount => _productionsCommands.Count;

        public ProductSliderController GetProductSliderPanel => _productSliderPanel;
        public ProductButtonPanelController GetProductionButtonsPanel => _productionButtonsPanel;
    }
}
