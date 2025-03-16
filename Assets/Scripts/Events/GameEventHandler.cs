using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    #region Main Menu Scene
    public static Action OnMainSceneStart;
    public static Action OnMainSceneExit;
    public static Action OnCompleteDataLoad;
    #endregion


    #region Farm Scene
    public static Action OnFarmSceneStart;
    public static Action OnFarmSceneExit;
    #endregion
}
