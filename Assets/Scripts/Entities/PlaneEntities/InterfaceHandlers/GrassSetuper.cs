using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Data.Models.DynamicData;
using Data.Models.FixedScriptableData;
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

        public void SetupEntity(GridSystem2D<GridObject<GrassAreaManager>> gridSystem, FixedEntityData fixedEntityData)
        {
            bool shouldBreakLoops = false;

            for (int y = 0; y < GridBoardManager.Instance.GetHeight; y++)
            {
                if (shouldBreakLoops) break;

                for (int x = 0; x < GridBoardManager.Instance.GetWidth; x++)
                {
                    if (gridSystem.GetValue(x, y) == null)
                    {
                        Vector3 targetGrassPos = gridSystem.GetWorldPositionCenter(x, y);

                        GameObject grassObject = Instantiate(fixedEntityData.EntityPrefab, targetGrassPos, Quaternion.identity, GridBoardManager.Instance.GetEntityLoader.transform);

                        GrassAreaManager grassAreaManager = grassObject.GetComponent<GrassAreaManager>();

                        grassObject.DoElasticStretch(_scaleFactor, _animationDuration, () => Debug.Log("callback"));

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

            grassAreaManager.grassAreaData = grassData;

            GameDataManager.Instance.GetGameDataReference.GrasAreaDatas.Add(grassData);

            GameDataManager.Instance.UpdatePlayerDataFile();
        }
    }
}
