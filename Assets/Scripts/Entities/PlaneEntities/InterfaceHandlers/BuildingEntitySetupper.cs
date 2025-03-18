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
    public class BuildingEntitySetupper : MonoBehaviour, IEntitySetup
    {
        public void SetupEntity(GridSystem2D<GridObject<GrassAreaManager>> gridSystem, FixedEntityData fixedEntityData)
        {
            bool shouldBreakLoops = false;

            for (int y = 0; y < GridBoardManager.Instance.GetHeight; y++)
            {
                if (shouldBreakLoops) break;

                for (int x = 0; x < GridBoardManager.Instance.GetWidth; x++)
                {
                    if (gridSystem.GetValue(x, y) == null)
                        return;

                    GrassAreaData grassAreaData = gridSystem.GetValue(x, y).GetValue().grassAreaData;

                    if (grassAreaData == null)
                        return;

                    if (grassAreaData.IsEmpty)
                    {
                        Vector3 targetPosition = gridSystem.GetWorldPositionCenter(x, y);

                        GameObject buildingEntity = Instantiate(fixedEntityData.EntityPrefab, targetPosition, Quaternion.identity);

                        buildingEntity.DoElasticStretch(new Vector3(0.5f, 2f, 0.5f), 1.5f, () => Debug.Log("entity spawned"));

                        grassAreaData.IsEmpty = false;

                        SaveBuildEntityData(fixedEntityData, grassAreaData);

                        shouldBreakLoops = true;
                        break;
                    }
                }
            }
        }

        private void SaveBuildEntityData(FixedEntityData fixedEntityData, GrassAreaData grassAreaData)
        {
            DynamicBuildingEntityData dynamicBuildingEntityData = new(0, fixedEntityData.EntityType, fixedEntityData); 

            GameDataManager.Instance.GetGameDataReference.BuildingEntityDatas.Add(dynamicBuildingEntityData);

            grassAreaData.DynamicBuildingEntityData = dynamicBuildingEntityData;

            GameDataManager.Instance.UpdatePlayerDataFile();  
        }
    }
}
