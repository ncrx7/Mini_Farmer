using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Entities;
using Entities.PlaneEntities;
using Enums;
using GridSystem.Models;
using UnityEngine;

namespace Data.Models.FixedScriptableData
{
    public class FixedBaseEntityData : ScriptableObject
    {
        public int EntityId;
        public string EntityName;
        public EntityType EntityType;

        [Header("Market Properties")]
        public int MarketPrice;
        public Sprite MarketSprite;
        public string MarketDescription;
        public EntityManager EntityPrefab;

        //public virtual void CreateEntityOnScene(GridSystem2D<GridObject<GrassAreaManager>> gridSystem2D) { }
    }
}
