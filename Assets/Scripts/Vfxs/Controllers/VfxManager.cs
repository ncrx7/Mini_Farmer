using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Vfx.Models;

namespace Vfx.Controllers
{
    public class VfxManager : MonoBehaviour
    {
        [SerializeField] private List<VisualEffect> _visualEffects;

        private void OnEnable()
        {
            GameEventHandler.PlayVfx += HandlePlayVfx;
        }

        private void OnDisable()
        {
            GameEventHandler.PlayVfx -= HandlePlayVfx;
        }

        private void HandlePlayVfx(Vector3 targetPosition, VfxType vfxType)
        {
            VisualEffect vfx = _visualEffects.Find(v => v.VfxType == vfxType);
            if (vfx != null && vfx.VfxPrefab != null)
            {
                GameObject spawnedVfx = Instantiate(vfx.VfxPrefab, targetPosition, Quaternion.identity);
                Destroy(spawnedVfx, vfx.Duration);
            }
            else
            {
                Debug.LogWarning("Undefined vfx TYPE!!");
            }
        }
    }
}
