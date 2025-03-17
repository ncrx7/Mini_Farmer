using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Data.Models.DynamicData;
using Data.Models.FixedScriptableData;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace Data.Controllers
{
    public class GameDataManager : SingletonBehavior<GameDataManager>
    {
        [SerializeField] private List<FixedStatData> _fixedStatDatas;
        [SerializeField] private List<FixedEntityData> _fixedEntityDatas;
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
            foreach (var stat in _gameData.StatDatas)
            {
                stat.FixedStatData = _fixedStatDatas.FirstOrDefault(fixedStatData => fixedStatData.StatId == stat.StatId);
            }

            await UniTask.Delay(1500);
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
            DynamicStatData levelData = new DynamicStatData(3, 0);
            DynamicStatData xpData = new DynamicStatData(4, 0);
            DynamicStatData moneyData = new DynamicStatData(5, 0);

            GameData gameData = new GameData(new List<DynamicStatData> { wheatData, flourData, breadData, moneyData, xpData, levelData }, new());


            return gameData;
        }

        public DynamicStatData GetDynamicStatData(StatType statType)
        {
            //return _gameData.StatDatas.First(data => data.FixedStatData.StatType == statType);
            return _statDataDictionary.Value.TryGetValue(statType, out var dynamicStatData) ? dynamicStatData : null;
        }

        public GameData GetGameDataReference => _gameData;
        public List<FixedEntityData> GetFixedEntityDatas => _fixedEntityDatas;
    }
}

public enum StatType
{
    Wheat,
    Flour,
    Bread,
    Level,
    Xp,
    Money
}

public enum EntityType
{
    Grass,
    FarmGranary,
    FarmMill,
    FarmStorage,
    BigHouse
}
