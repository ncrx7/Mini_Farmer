using System;
using System.Collections;
using System.Collections.Generic;
using Data.Models.FixedScriptableData;
using UnityEngine;

namespace Data.Models.DynamicData
{
    [Serializable]
    public class DynamicStatData
    {
        public int StatId;
        public int Amount;
        public FixedStatData FixedStatData;
    }
}
