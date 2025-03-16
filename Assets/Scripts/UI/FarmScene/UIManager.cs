using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Data.Models.DynamicData;
using TMPro;
using UI.FarmerScene.EntityUIController;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace UI.FarmerScene
{
    public class UIManager : BaseUIManager
    {
        [SerializeField] private StatUI _statEmptyPrefab;
        [SerializeField] private Transform _statLayout;
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private TextMeshProUGUI _loadingPanelText;

        private void OnEnable()
        {
            GameEventHandler.OnStartEntitesLoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartEntitesLoad += () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Entities Loading", _loadingPanelText);

            GameEventHandler.OnCompleteEntitiesLoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, false, _loadingPanel);
        }

        private void OnDisable()
        {
            GameEventHandler.OnStartEntitesLoad -= () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartEntitesLoad -= () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Entities Loading", _loadingPanelText);

            GameEventHandler.OnCompleteEntitiesLoad -= () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, false, _loadingPanel);
        }

        private void Start()
        {
            InitializeStatUIs();
        }

        private void InitializeStatUIs()
        {
            foreach (DynamicStatData statData in GameDataManager.Instance.GetGameDataReference.StatDatas)
            {
                StatUI statUI = Instantiate(_statEmptyPrefab, _statLayout);

                statUI.GetStatAmountText.text = statData.Amount.ToString();
                statUI.GetStatIconImage.sprite = statData.FixedStatData.StatSprite;
            }
        }
    }
}