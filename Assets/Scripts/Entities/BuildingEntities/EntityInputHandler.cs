using System;
using Data.Controllers;
using Entities.BuildingEntities;
using Entities.BuildingEntities.InterfaceHandlers;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entities.Input
{
    public class EntityInputHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private BuildingEntityManager _entityManager;
        private ICollect _produceCollector;
        private Action _collectProduceCallback;

        private void Start()
        {
            if (_entityManager.entityData.FixedBuildingEntityData.EntityType == EntityType.FarmGranary)
                _produceCollector = new CollectProducesWithoutResources();
            else
                _produceCollector = new CollectProducesWithResources();

            _collectProduceCallback = () =>
            {
                GameDataManager.Instance.GetDynamicStatData(_entityManager.entityData.FixedBuildingEntityData.ProductionProcut.StatType).Amount += _entityManager.entityData.CurrentProductInStorage;
                _entityManager.entityData.CurrentProductInStorage = 0;
                GameDataManager.Instance.UpdatePlayerDataFile();
            };
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _produceCollector.TryCollectProduces(_entityManager, _collectProduceCallback);

            GameEventHandler.OnClickEntity?.Invoke(_entityManager);
        }
    }
}