using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Models.FixedScriptableData
{
    [CreateAssetMenu(fileName = "StatData", menuName = "Scriptable Objects/StatData")]
    public class FixedStatData : ScriptableObject
    {
        public int StatId;
        public StatType StatType;
        public string StatName;
        public string StatDescription;
        public Sprite StatSprite;
    }
}
