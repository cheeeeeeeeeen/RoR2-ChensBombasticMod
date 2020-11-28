using RoR2;
using System.Collections.Generic;
using UnityEngine;
using static RoR2.Artifacts.BombArtifactManager;

namespace Chen.BombasticMod
{
    internal class BombasticManager : MonoBehaviour
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

        public void QueueBomb(CharacterBody body)
        {
            Vector3 corePosition = body.corePosition;
            float bombComputation = body.bestFitRadius * extraBombPerRadius * cvSpiteBombCoefficient.value;
            int num = Mathf.Min(maxBombCount, Mathf.CeilToInt(bombComputation));
            for (int i = 0; i < num; i++)
            {
                Vector3 b = Random.insideUnitSphere * (bombSpawnBaseRadius + body.bestFitRadius * bombSpawnRadiusCoefficient);
                BombRequest item = new BombRequest
                {
                    spawnPosition = corePosition,
                    raycastOrigin = corePosition + b,
                    bombBaseDamage = body.damage * bombDamageCoefficient,
                    attacker = body.gameObject,
                    teamIndex = body.teamComponent.teamIndex,
                    velocityY = Random.Range(5f, 25f)
                };
                bombRequestQueue.Enqueue(item);
            }
        }
    }
}