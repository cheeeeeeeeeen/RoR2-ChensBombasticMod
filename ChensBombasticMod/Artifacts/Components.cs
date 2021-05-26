using Chen.Helpers.UnityHelpers;
using RoR2;
using UnityEngine;
using static RoR2.Artifacts.BombArtifactManager;

namespace Chen.BombasticMod
{
    internal class BombasticManager : QueueProcessor<BombRequest>
    {
        protected override int itemsPerFrame { get; set; } = 1;

        protected override float processInterval { get; set; } = 0;

        protected override bool Process(BombRequest bombRequest)
        {
            Ray ray = new Ray(bombRequest.raycastOrigin + new Vector3(0f, maxBombStepUpDistance, 0f), Vector3.down);
            float maxDistance = maxBombStepUpDistance + maxBombFallDistance;
            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
            {
                SpawnBomb(bombRequest, raycastHit.point.y);
            }
            return true;
        }

        public virtual void QueueBomb(CharacterBody body)
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
                Add(item);
            }
        }
    }

    internal class SpleenManager : BombasticManager
    {
        private int limit { get; set; } = Spleen.instance.spleenQueueLimit;

        protected override float processInterval { get; set; } = Spleen.instance.spleenQueueProcessingInterval;

        public override void QueueBomb(CharacterBody body)
        {
            if (processQueue.Count >= limit) return;
            base.QueueBomb(body);
        }
    }
}