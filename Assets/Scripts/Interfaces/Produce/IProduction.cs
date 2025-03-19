using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Models.DynamicData;
using UnityEngine;

namespace Interfaces
{
    public interface IProduction<T>
    {
        public int CurrentProductionTime {get; set;}
        public UniTask StartProduction(T entityData);
    }
}
