using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entities
{
    public class EntityClickReset : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            GameEventHandler.OnClickReset?.Invoke();
        }
    }
}
