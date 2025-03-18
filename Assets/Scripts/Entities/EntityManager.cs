using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EntityManager : MonoBehaviour
    {
        
    }

    public class EntityManager<T> : EntityManager
    {
        public T entityData;
    }
}
