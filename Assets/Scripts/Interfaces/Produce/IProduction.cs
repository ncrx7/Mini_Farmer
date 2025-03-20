using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Models.DynamicData;
using Entities;
using UnityEngine;

namespace Interfaces
{
    public interface IProduction<T>
    {
        public UniTask StartProduction(T entityData, EntityManager<T> entityManager);
    }
}
