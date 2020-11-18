using RoR2;
using System.Collections.Generic;
using UnityEngine;
using static RoR2.Artifacts.BombArtifactManager;

namespace Chen.BombasticMod
{
    public class BombasticManager : MonoBehaviour
    {
        public readonly Queue<BombRequest> bombRequestQueue = new Queue<BombRequest>();

        private void FixedUpdate()
        {
            if (bombRequestQueue.Count > 0)
            {
                BombRequest bombRequest = bombRequestQueue.Dequeue();
                Ray ray = new Ray(bombRequest.raycastOrigin + new Vector3(0f, maxBombStepUpDistance, 0f), Vector3.down);
                float maxDistance = maxBombStepUpDistance + maxBombFallDistance;
                if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
                {
                    SpawnBomb(bombRequest, raycastHit.point.y);
                }
            }
        }
    }
}