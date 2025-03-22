using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;

namespace UI.MainScene
{
    public class UIManager : BaseUIManager
    {
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private TextMeshProUGUI _loadingPanelText;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        private void OnEnable()
        {
            GameEventHandler.OnStartDataLoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartDataLoad += () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Game Data Loading", _loadingPanelText);

            GameEventHandler.OnCompleteDataLoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, false, _loadingPanel);

            GameEventHandler.OnClickStartButton += (sceneId) => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnClickStartButton += (sceneId) => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Loading Scene", _loadingPanelText);
        }

        private void OnDisable()
        {
            GameEventHandler.OnStartDataLoad -= () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartDataLoad -= () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Game Data Loading", _loadingPanelText);

            GameEventHandler.OnCompleteDataLoad -= () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, false, _loadingPanel);

            GameEventHandler.OnClickStartButton -= (sceneId) => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnClickStartButton -= (sceneId) => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Loading Scene", _loadingPanelText);
        }

        protected override void Awake()
        {
            base.Awake();

            _startButton.onClick.AddListener(() => GameEventHandler.OnClickStartButton?.Invoke(1));
        }
    }
}
