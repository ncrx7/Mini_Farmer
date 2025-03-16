using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Models.DynamicData
{
    [Serializable]
    public class GameData
    {
        public List<DynamicStatData> StatDatas;
        public List<GrassAreaData> GrasAreaDatas;

        public GameData(List<DynamicStatData> statDatas, List<GrassAreaData> grassAreaDatas)
        {
            StatDatas = statDatas;
            GrasAreaDatas = grassAreaDatas;
        }
    }
}
