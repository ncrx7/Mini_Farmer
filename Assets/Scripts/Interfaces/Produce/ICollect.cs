using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Interfaces
{
    public interface ICollect
    {
        public void TryCollectProduces(EntityManager entityManager, Action callBack);
    }
}
