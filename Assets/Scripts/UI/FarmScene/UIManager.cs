using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Data.Models.DynamicData;
using UI.FarmerScene.EntityUIController;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace UI.FarmerScene
{
    public class UIManager : BaseUIManager
    {
        [SerializeField] private StatUI _statEmptyPrefab;
        [SerializeField] private Transform _statLayout;

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