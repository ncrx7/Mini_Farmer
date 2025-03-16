using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace UI.MainScene
{
    public class UIManager : BaseUIManager
    {
        [SerializeField] private GameObject _loadingPanel;

        private void OnEnable()
        {
            ExecuteUIAction(UIActionType.SetMainMenuLoadingPanel, true);

            GameEventHandler.OnCompleteDataLoad += () => ExecuteUIAction(UIActionType.SetMainMenuLoadingPanel, false);
        }

        private void OnDisable()
        {
            GameEventHandler.OnCompleteDataLoad -= () => ExecuteUIAction(UIActionType.SetMainMenuLoadingPanel, false);
        }

        private void Awake()
        {
            _uiActionMap = new Dictionary<UIActionType, Action<bool>>
            {
                {UIActionType.SetMainMenuLoadingPanel, (active) => _loadingPanel.SetActive(active)},
            };
        }
    }
}
