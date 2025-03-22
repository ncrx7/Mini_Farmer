using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Data.Models.DynamicData;
using Data.Models.FixedScriptableData;
using Enums;
using Interfaces;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;

namespace Entities.PlaneEntities.InterfaceHandlers
{
    public class GrassSetuper : MonoBehaviour, IEntitySetup
    {
        [SerializeField] private GrassAreaManager _grassAreaManager;

        [Header("Entity Create Animation Settings")]
        [SerializeField] private Vector3 _scaleFactor;
        [SerializeField] private float _animationDuration;

        public void SetupEntity(GridSystem2D<GridObject<GrassAreaManager>> gridSystem, FixedBaseEntityData fixedEntityData)
        {
            bool shouldBreakLoops = false;

            for (int y = 0; y < GridBoardManager.Instance.GetHeight; y++)
            {
                if (shouldBreakLoops) break;

                for (int x = 0; x < GridBoardManager.Instance.GetWidth; x++)
                {
                    if (gridSystem.GetValue(x, y) == null && CheckMoneyEnough(fixedEntityData))
                    {
                        PayPrice(fixedEntityData);

                        GameEventHandler.OnCreateEntity?.Invoke(fixedEntityData, GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount);

                        Vector3 targetGrassPos = gridSystem.GetWorldPositionCenter(x, y);

                        EntityManager grassObject = Instantiate(fixedEntityData.EntityPrefab, targetGrassPos, Quaternion.identity, GridBoardManager.Instance.GetEntityLoader.transform);

                        GrassAreaManager grassAreaManager = grassObject.GetComponent<GrassAreaManager>();

                        grassObject.gameObject.DoElasticStretch(_scaleFactor, _animationDuration, () => Debug.Log("callback"));

                        GameEventHandler.PlaySoundClip(SoundType.PurchaseEntity);

                        GridObject<GrassAreaManager> gridObject = new(gridSystem, x, y);
                        gridObject.SetValue(grassAreaManager);
                        gridSystem.SetValue(x, y, gridObject);

                        grassObject.transform.position = targetGrassPos;

                        SaveGrassData(x, y, grassAreaManager);

                        shouldBreakLoops = true;
                        break;
                    }
                }
            }
        }

        private void SaveGrassData(int x, int y, GrassAreaManager grassAreaManager)
        {
            GrassAreaData grassData = new GrassAreaData();
            grassData.XGridPosition = x;
            grassData.YGridPosition = y;
            grassData.IsEmpty = true;

            grassAreaManager.entityData = grassData;

            GameDataManager.Instance.GetGameDataReference.GrasAreaDatas.Add(grassData);

            GameDataManager.Instance.UpdatePlayerDataFile();
        }

        private void PayPrice(FixedBaseEntityData fixedEntityData)
        {
            GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount -= fixedEntityData.MarketPrice;
        }

        private bool CheckMoneyEnough(FixedBaseEntityData fixedEntityData)
        {
            return GameDataManager.Instance.GetDynamicStatData(StatType.Money).Amount >= fixedEntityData.MarketPrice;
        }
    }
}
