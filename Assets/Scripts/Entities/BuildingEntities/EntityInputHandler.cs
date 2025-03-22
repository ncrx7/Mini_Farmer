using Data.Controllers;
using Entities.BuildingEntities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entities.Input
{
    public class EntityInputHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private BuildingEntityManager _entityManager;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_entityManager.GetProductionButtonsPanel.gameObject.activeSelf)
            {
                GameDataManager.Instance.GetDynamicStatData(_entityManager.entityData.FixedBuildingEntityData.ProductionProcut.StatType).Amount += _entityManager.entityData.CurrentProductInStorage;
                _entityManager.entityData.CurrentProductInStorage = 0;
                GameDataManager.Instance.UpdatePlayerDataFile();
            }
            
            GameEventHandler.OnClickEntity?.Invoke(_entityManager);


            Debug.Log("On pointer Click worked");
        }
    }
}