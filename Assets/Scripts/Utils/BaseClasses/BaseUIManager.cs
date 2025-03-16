using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.BaseClasses
{
    public class BaseUIManager : MonoBehaviour
    {
        protected Dictionary<UIActionType, object> _uiActionMap = new();

        protected void AddUIAction<T>(UIActionType actionType, Action<T> action)
        {
            _uiActionMap[actionType] = action;
        }

        protected void AddUIAction<T1, T2>(UIActionType actionType, Action<T1, T2> action)
        {
            _uiActionMap[actionType] = action;
        }

        protected void ExecuteUIAction<T>(UIActionType actionType, T value)
        {
            if (_uiActionMap.TryGetValue(actionType, out var action) && action is Action<T> typedAction)
            {
                typedAction(value);
            }
            else
            {
                Debug.LogWarning("Undefined action Type!!");
            }
        }

        protected void ExecuteUIAction<T1, T2>(UIActionType actionType, T1 value1, T2 value2)
        {
            if (_uiActionMap.TryGetValue(actionType, out var action) && action is Action<T1, T2> typedAction)
            {
                typedAction(value1, value2);
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
    SetMainMenuLoadingPanel,
    SetText
}

