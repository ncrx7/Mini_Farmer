using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Data.Models.DynamicData;
using Interfaces;
using NodeGridSystem.Controllers;
using TMPro;
using UI.FarmerScene.EntityUIController;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace UI.FarmerScene
{
    public class UIManager : BaseUIManager
    {
        [SerializeField] private StatUI _statEmptyPrefab;
        private Dictionary<StatType, TextMeshProUGUI> _statTexts = new();

        [SerializeField] private MarketUI _entityMarketPrefab;

        [SerializeField] private Transform _statLayout;
        [SerializeField] private Transform _marketObjLayout;

        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private TextMeshProUGUI _loadingPanelText;

        private void OnEnable()
        {
            GameEventHandler.OnStartEntitesLoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartEntitesLoad += () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Entities Loading", _loadingPanelText);

            GameEventHandler.OnStartStatUILoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartStatUILoad += () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Stat UIs Loading", _loadingPanelText);

            GameEventHandler.OnCompleteStatUILoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, false, _loadingPanel);

            GameEventHandler.OnCreateEntity += (fixedEntityData, money) => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, money.ToString(), _statTexts[StatType.Money]);
            
            GameEventHandler.OnBuildingEntitySpawnOnScene += (productInStorageText, productTimeText, productQueueAmountText, buildingEntityData) =>
            { 
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, buildingEntityData.CurrentProductInStorage.ToString(), productInStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "40s" , productTimeText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "4/10", productQueueAmountText);
            };
        }

        private void OnDisable()
        {
            GameEventHandler.OnStartEntitesLoad -= () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartEntitesLoad -= () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Entities Loading", _loadingPanelText);

            GameEventHandler.OnStartStatUILoad -= () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartStatUILoad -= () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Stat UIs Loading", _loadingPanelText);

            GameEventHandler.OnCompleteStatUILoad -= () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, false, _loadingPanel);

            GameEventHandler.OnCreateEntity -= (fixedEntityData, money) => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, money.ToString(), _statTexts[StatType.Money]);
        
            GameEventHandler.OnBuildingEntitySpawnOnScene -= (productInStorageText, productTimeText, productQueueAmountText, buildingEntityData) =>
            { 
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, buildingEntityData.CurrentProductInStorage.ToString(), productInStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "40s" , productTimeText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "4/10", productQueueAmountText);
            };
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            InitializeStatUIs().Forget();
            InitializeMarketUI().Forget();
        }

        private async UniTask InitializeStatUIs()
        {
            await UniTask.WaitUntil(() => GridBoardManager.Instance.GetEntityLoader.AreEntitiesLoadFinished == true);

            GameEventHandler.OnStartStatUILoad?.Invoke();

            foreach (DynamicStatData statData in GameDataManager.Instance.GetGameDataReference.StatDatas)
            {
                StatUI statUI = Instantiate(_statEmptyPrefab, _statLayout);

                statUI.GetStatAmountText.text = statData.Amount.ToString();
                statUI.GetStatIconImage.sprite = statData.FixedStatData.StatSprite;

                _statTexts.Add(statData.FixedStatData.StatType, statUI.GetStatAmountText);

                await UniTask.Delay(300);
            }

            GameEventHandler.OnCompleteStatUILoad?.Invoke();
        }

        private async UniTask InitializeMarketUI()
        {
            await UniTask.WaitUntil(() => GridBoardManager.Instance.GetEntityLoader.AreEntitiesLoadFinished == true);

            //GameEventHandler.OnStartStatUILoad?.Invoke();

            foreach (var entityData in GameDataManager.Instance.GetFixedEntityDatas)
            {
                MarketUI marketUI = Instantiate(_entityMarketPrefab, _marketObjLayout);

                marketUI.GetMarketImage.sprite = entityData.MarketSprite;
                marketUI.GetPriceText.text = entityData.MarketPrice.ToString() + "$";
                marketUI.GetPurchaseText.text = "Purchase";
   
                marketUI.GetPurchaseButton.onClick.AddListener(() =>
                {
                    if (entityData.EntityPrefab.TryGetComponent<IEntitySetup>(out var entitySetup))
                    {
                        entitySetup.SetupEntity(GridBoardManager.Instance.GetNodeGridSystem2D, entityData);
                    }
                });
            }

            //GameEventHandler.OnCompleteStatUILoad?.Invoke();
        }
    }
}