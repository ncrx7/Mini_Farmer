using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Data.Models.DynamicData;
using Enums;
using Interfaces;
using GridSystem.Controllers;
using TMPro;
using UI.FarmerScene.EntityUIController;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;

namespace UI.FarmerScene
{
    public class UIManager : BaseUIManager
    {
        [Header("Store UI")]
        [SerializeField] private GameObject _storePanel;
        [SerializeField] private Button _storeButton;
        [SerializeField] private MarketUI _entityMarketPrefab;
        [SerializeField] private Transform _marketObjLayout;

        [Header("Stat UI")]
        [SerializeField] private StatUI _statEmptyPrefab;
        private Dictionary<StatType, TextMeshProUGUI> _statTexts = new();
        [SerializeField] private Transform _statLayout;

        [Header("Loading UI")]
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private TextMeshProUGUI _loadingPanelText;

        private GameObject _activeProductionButtonPanel;

        private void OnEnable()
        {
            GameEventHandler.OnStartEntitesLoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartEntitesLoad += () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Entities Loading", _loadingPanelText);

            GameEventHandler.OnStartStatUILoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, true, _loadingPanel);
            GameEventHandler.OnStartStatUILoad += () => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "Stat UIs Loading", _loadingPanelText);

            GameEventHandler.OnCompleteStatUILoad += () => ExecuteUIAction<bool, GameObject>(UIActionType.SetMainMenuLoadingPanel, false, _loadingPanel);

            GameEventHandler.OnCreateEntity += (fixedEntityData, money) => ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, money.ToString(), _statTexts[StatType.Money]);

            GameEventHandler.OnBuildingEntitySpawnOnScene += (buildEntityManager, currentStorage, storageCapacityRate, productIcon, buttonIncreaseProductionEvent, buttonReduceProductionEvent) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage, buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "", buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText);
                ExecuteUIAction<Slider, float>(UIActionType.SetSlider, buildEntityManager.GetProductSliderPanel.GetSlider, 1);
                ExecuteUIAction<Image, Sprite>(UIActionType.SetImage, buildEntityManager.GetProductSliderPanel.GetProductionProduceImage, productIcon);

                if (buildEntityManager.GetProductionButtonsPanel == null)
                {
                    buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText.gameObject.SetActive(false);
                    return;
                }

                buildEntityManager.GetProductionButtonsPanel.GetIncreaseButton.onClick.AddListener(() => buttonIncreaseProductionEvent?.Invoke());
                buildEntityManager.GetProductionButtonsPanel.GetReduceButton.onClick.AddListener(() => buttonReduceProductionEvent?.Invoke());

                buildEntityManager.GetProductionButtonsPanel.GetResourceIcon.sprite =  buildEntityManager.entityData.FixedBuildingEntityData.ResourceProduct.StatSprite;
                buildEntityManager.GetProductionButtonsPanel.GetResourceAmountText.text = "x" + buildEntityManager.entityData.FixedBuildingEntityData.ResourceAmount;
            };

            GameEventHandler.OnProductionStart += (buildEntityManager, currentRemainTime, storageCapacityRate) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentRemainTime.ToString() + "s", buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText); //ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(statType).Amount.ToString(), _statTexts[statType]);
            };

            GameEventHandler.OnProductionContinue += (buildEntityManager, currentRemainTime, sliderValue) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentRemainTime.ToString() + "s", buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<Slider, float>(UIActionType.SetSlider, buildEntityManager.GetProductSliderPanel.GetSlider, sliderValue);
            };

            GameEventHandler.OnProductionEnd += (buildEntityManager, currentStorage, currentTime) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage.ToString(), buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentTime.ToString(), buildEntityManager.GetProductSliderPanel.GetProductTimeText);
            };

            GameEventHandler.OnCompleteCalculateProductByElapsedTime += (buildEntityManager, currentStorage, storageCapacityRate) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage.ToString(), buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "", buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<Slider, float>(UIActionType.SetSlider, buildEntityManager.GetProductSliderPanel.GetSlider, 1);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText);
            };

            GameEventHandler.OnClickEntity += (buildEntityManager, currentStorage, statType, storageCapacityRate) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage.ToString(), buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(statType).Amount.ToString(), _statTexts[statType]);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText);

                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount.ToString(), _statTexts[StatType.Money]);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(StatType.Xp).Amount.ToString(), _statTexts[StatType.Xp]);

                if (buildEntityManager.GetProductionButtonsPanel == null)
                    return;

                if (_activeProductionButtonPanel != null)
                    _activeProductionButtonPanel.SetActive(false);

                _activeProductionButtonPanel = buildEntityManager.GetProductionButtonsPanel.gameObject;
                _activeProductionButtonPanel.SetActive(true);
            };

            GameEventHandler.OnClickReset += () =>
            {
                if (_activeProductionButtonPanel != null)
                    _activeProductionButtonPanel.SetActive(false);
            };

            GameEventHandler.OnClickIncreaseButton += (statAmount, statType) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, statAmount, _statTexts[statType]);
            };

            GameEventHandler.OnClickReduceButton += (statAmount, statType) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, statAmount, _statTexts[statType]);
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

            GameEventHandler.OnBuildingEntitySpawnOnScene -= (buildEntityManager, currentStorage, storageCapacityRate, productIcon, buttonIncreaseProductionEvent, buttonReduceProductionEvent) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage, buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "", buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText);
                ExecuteUIAction<Slider, float>(UIActionType.SetSlider, buildEntityManager.GetProductSliderPanel.GetSlider, 1);
                ExecuteUIAction<Image, Sprite>(UIActionType.SetImage, buildEntityManager.GetProductSliderPanel.GetProductionProduceImage, productIcon);

                if (buildEntityManager.GetProductionButtonsPanel == null)
                {
                    buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText.gameObject.SetActive(false);
                    return;
                }

                buildEntityManager.GetProductionButtonsPanel.GetIncreaseButton.onClick.AddListener(() => buttonIncreaseProductionEvent?.Invoke());
                buildEntityManager.GetProductionButtonsPanel.GetReduceButton.onClick.AddListener(() => buttonReduceProductionEvent?.Invoke());

                buildEntityManager.GetProductionButtonsPanel.GetResourceIcon.sprite =  buildEntityManager.entityData.FixedBuildingEntityData.ResourceProduct.StatSprite;
                buildEntityManager.GetProductionButtonsPanel.GetResourceAmountText.text = "x" + buildEntityManager.entityData.FixedBuildingEntityData.ResourceAmount;
            };

            GameEventHandler.OnProductionStart -= (buildEntityManager, currentRemainTime, storageCapacityRate) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentRemainTime.ToString() + "s", buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText); //ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(statType).Amount.ToString(), _statTexts[statType]);
            };

            GameEventHandler.OnProductionContinue -= (buildEntityManager, currentTime, sliderValue) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentTime.ToString(), buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<Slider, float>(UIActionType.SetSlider, buildEntityManager.GetProductSliderPanel.GetSlider, sliderValue);
            };

            GameEventHandler.OnProductionEnd -= (buildEntityManager, currentStorage, currentTime) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage.ToString(), buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentTime.ToString(), buildEntityManager.GetProductSliderPanel.GetProductTimeText);
            };

            GameEventHandler.OnCompleteCalculateProductByElapsedTime -= (buildEntityManager, currentStorage, storageCapacityRate) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage.ToString(), buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, "", buildEntityManager.GetProductSliderPanel.GetProductTimeText);
                ExecuteUIAction<Slider, float>(UIActionType.SetSlider, buildEntityManager.GetProductSliderPanel.GetSlider, 1);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText);
            };

            GameEventHandler.OnClickEntity -= (buildEntityManager, currentStorage, statType, storageCapacityRate) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, currentStorage.ToString(), buildEntityManager.GetProductSliderPanel.GetCurrentStorageText);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(statType).Amount.ToString(), _statTexts[statType]);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, storageCapacityRate, buildEntityManager.GetProductSliderPanel.GetStorageCapacityRateText);
                
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount.ToString(), _statTexts[StatType.Money]);
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, GameDataManager.Instance.GetDynamicStatData(StatType.Xp).Amount.ToString(), _statTexts[StatType.Xp]);

                if (buildEntityManager.GetProductionButtonsPanel == null)
                    return;

                if (_activeProductionButtonPanel != null)
                    _activeProductionButtonPanel.SetActive(false);

                _activeProductionButtonPanel = buildEntityManager.GetProductionButtonsPanel.gameObject;
                _activeProductionButtonPanel.SetActive(true);
            };

            GameEventHandler.OnClickReset -= () =>
            {
                if (_activeProductionButtonPanel != null)
                    _activeProductionButtonPanel.SetActive(false);
            };

            GameEventHandler.OnClickIncreaseButton -= (statAmount, statType) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, statAmount, _statTexts[statType]);
            };

            GameEventHandler.OnClickReduceButton -= (statAmount, statType) =>
            {
                ExecuteUIAction<string, TextMeshProUGUI>(UIActionType.SetText, statAmount, _statTexts[statType]);
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

            _storeButton.onClick.AddListener(OnStoreButtonClick);
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

        private void OnStoreButtonClick()
        {
            if (_storePanel == null)
                return;

            _storePanel.SetActive(!_storePanel.activeSelf);
        }
    }
}