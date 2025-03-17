using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Data.Models.FixedScriptableData
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]

    public class FixedEntityData : ScriptableObject
    {
        public int EntityId;
        public string EntityName;
        public EntityType EntityType;

        [Header("Market Properties")]
        public int MarketPrice;
        public Sprite MarketSprite;
        public string MarketDescription;

        [Header("Building Properties")]
        public GameObject EntityPrefab;
        
    }
}
