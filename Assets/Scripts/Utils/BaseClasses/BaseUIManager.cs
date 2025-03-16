using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.BaseClasses
{
    public class BaseUIManager : MonoBehaviour
    {
        protected Dictionary<UIActionType, Action<bool>> _uiActionMap;

        protected void ExecuteUIAction(UIActionType actionType, bool active)
        {
            if (_uiActionMap.TryGetValue(actionType, out var action))
            {
                action.Invoke(active);
            }
            else
            {
                Debug.LogWarning("Undefined action Type!!");
            }
        }
    }
}

public enum UIActionType
{
    SetMainMenuLoadingPanel
}

