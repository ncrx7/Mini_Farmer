using System;
using System.Collections;
using System.Collections.Generic;
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
            ExecuteUIAction(UIActionType.SetMainMenuLoadingPanel, true);
            ExecuteUIAction(UIActionType.SetText, "Game Data Loading", _loadingPanelText);

            GameEventHandler.OnCompleteDataLoad += () => ExecuteUIAction<bool>(UIActionType.SetMainMenuLoadingPanel, false);

            GameEventHandler.OnClickStartButton += (sceneId) => ExecuteUIAction<bool>(UIActionType.SetMainMenuLoadingPanel, true);
            GameEventHandler.OnClickStartButton += (sceneId) => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Loading Scene", _loadingPanelText);
        }

        private void OnDisable()
        {
            GameEventHandler.OnCompleteDataLoad -= () => ExecuteUIAction<bool>(UIActionType.SetMainMenuLoadingPanel, false);

            GameEventHandler.OnClickStartButton -= (sceneId) => ExecuteUIAction<bool>(UIActionType.SetMainMenuLoadingPanel, true);
            GameEventHandler.OnClickStartButton -= (sceneId) => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Loading Scene", _loadingPanelText);
        }

        private void Awake()
        {

            AddUIAction<bool>(UIActionType.SetMainMenuLoadingPanel, (active) => _loadingPanel.SetActive(active));
            AddUIAction<string, TextMeshProUGUI>(UIActionType.SetText, (textString, textObject) => textObject.text = textString);

            _startButton.onClick.AddListener(() => GameEventHandler.OnClickStartButton?.Invoke(1));
        }
    }
}
