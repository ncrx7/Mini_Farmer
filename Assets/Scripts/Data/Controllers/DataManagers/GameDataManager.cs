using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Data.Models.DynamicData;
using Data.Models.FixedScriptableData;
using Enums;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace Data.Controllers
{
    public class GameDataManager : SingletonBehavior<GameDataManager>
    {
        [Header("Starter Stats")]
        [SerializeField] private int _starterMoney;

        [Header("Fixed Datas")]
        [SerializeField] private List<FixedStatData> _fixedStatDatas;
        [SerializeField] private List<FixedBaseEntityData> _fixedEntityDatas;
        [SerializeField] private GameData _gameData; //**TODO: MAKE HERE READY ONLY NOT SERIALIZEFIELD**
        DataWriterAndReader<GameData> _dataWriterAndReader;
        public bool AreDataLoadFinished = false;
        private Lazy<Dictionary<StatType, DynamicStatData>> _statDataDictionary;

        private void Awake()
        {
            transform.parent = null;

            DontDestroyOnLoad(gameObject);

            _dataWriterAndReader = new DataWriterAndReader<GameData>(Application.persistentDataPath, "Game_Data");
        }

        private void Start()
        {
            InitializeData();
        }

        private async void InitializeData()
        {
            GameEventHandler.OnStartDataLoad?.Invoke();

            await LoadGameDataFile();
            await LoadStatFixedData();
            await LoadEntityFixedData();

            AreDataLoadFinished = true;

            GameEventHandler.OnCompleteDataLoad?.Invoke();
        }

        private async UniTask LoadGameDataFile()
        {
            _gameData = await _dataWriterAndReader.InitializeDataFile();

            _statDataDictionary = new Lazy<Dictionary<StatType, DynamicStatData>>(
                () => _gameData.StatDatas.ToDictionary(data => data.FixedStatData.StatType)
            );
        }

        private async UniTask LoadStatFixedData()
        {
            foreach (var statData in _gameData.StatDatas)
            {
                statData.FixedStatData = _fixedStatDatas.FirstOrDefault(fixedStatData => fixedStatData.StatId == statData.StatId);
            }

            await UniTask.Delay(700);
        }

        private async UniTask LoadEntityFixedData()
        {
            foreach (var buildingEntityData in _gameData.BuildingEntityDatas)
            {
                buildingEntityData.FixedBuildingEntityData = _fixedEntityDatas.First(fixedEntityData => fixedEntityData.EntityType == buildingEntityData.EntityType) as FixedBuildingEntityData;
            }

            await UniTask.Delay(700);

            foreach (var grassEntityData in _gameData.GrasAreaDatas)
            {
                grassEntityData.DynamicBuildingEntityData.FixedBuildingEntityData = _fixedEntityDatas.First
                                                        (fixedEntityData => fixedEntityData.EntityType == grassEntityData.DynamicBuildingEntityData.EntityType) as FixedBuildingEntityData;;
            }

            await UniTask.Delay(700);
        }

        public void UpdatePlayerDataFile()
        {
            _dataWriterAndReader.UpdateDataFile(_gameData);
        }

        public GameData CreateNewGameDataObject()
        {
            DynamicStatData wheatData = new DynamicStatData(0, 0);
            DynamicStatData flourData = new DynamicStatData(1, 0);
            DynamicStatData breadData = new DynamicStatData(2, 0);
            DynamicStatData levelData = new DynamicStatData(3, 1);
            DynamicStatData xpData = new DynamicStatData(4, 0);
            DynamicStatData moneyData = new DynamicStatData(5, _starterMoney);

            GameData gameData = new GameData(new List<DynamicStatData> { wheatData, flourData, breadData, moneyData, xpData, levelData }, new() { }, new() { });

            return gameData;
        }

        public DynamicStatData GetDynamicStatData(StatType statType)
        {
            //return _gameData.StatDatas.First(data => data.FixedStatData.StatType == statType);
            return _statDataDictionary.Value.TryGetValue(statType, out var dynamicStatData) ? dynamicStatData : null;
        }

        public GameData GetGameDataReference => _gameData;
        public List<FixedBaseEntityData> GetFixedEntityDatas => _fixedEntityDatas;
    }
}
