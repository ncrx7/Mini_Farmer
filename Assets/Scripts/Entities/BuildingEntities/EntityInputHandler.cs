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
            GameEventHandler.OnClickEntity?.Invoke(_entityManager);

           // if(_entityManager.GetProductionButtonsPanel.activeSelf)
                //TODO: COLLECT PRODUCTS

            //_entityManager.GetProductionButtonsPanel.SetActive(true);
            Debug.Log("On pointer Click worked");
        }
    }
}