using System;
using System.Collections;
using System.Collections.Generic;
using Data.Models.DynamicData;
using Data.Models.FixedScriptableData;
using Entities;
using Entities.BuildingEntities;
using Entities.PlaneEntities;
using NodeGridSystem.Models;
using TMPro;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    #region Main Menu Scene
    public static Action OnMainSceneStart;
    public static Action OnMainSceneExit;
    public static Action OnStartDataLoad;
    public static Action OnCompleteDataLoad;
    public static Action<int> OnClickStartButton;
    #endregion


    #region Farm Scene
    public static Action OnFarmSceneStart;
    public static Action OnFarmSceneExit;

    public static Action OnStartEntitesLoad;
    public static Action OnCompleteEntitiesLoad;

    public static Action OnStartStatUILoad;
    public static Action OnCompleteStatUILoad;

    public static Action<FixedBaseEntityData, int> OnCreateEntity;

    public static Action<BuildingEntityManager, string, string, Sprite, Action, Action> OnBuildingEntitySpawnOnScene;

    #region Production Events
    public static Action<BuildingEntityManager, int, string> OnProductionStart;
    public static Action<BuildingEntityManager, int, float> OnProductionContinue;
    public static Action<BuildingEntityManager, int, int> OnProductionEnd;
    public static Action<BuildingEntityManager, string, string> OnCompleteCalculateProductByElapsedTime;
    public static Action<string, StatType> OnClickIncreaseButton;
    #endregion

    public static Action<BuildingEntityManager, string, StatType, string> OnClickEntity;
    public static Action OnClickReset;
    #endregion
}
