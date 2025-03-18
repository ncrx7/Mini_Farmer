using System;
using System.Collections;
using System.Collections.Generic;
using Data.Models.FixedScriptableData;
using Entities.PlaneEntities;
using NodeGridSystem.Models;
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

    public static Action<FixedEntityData, int> OnCreateEntity;
    #endregion
}
