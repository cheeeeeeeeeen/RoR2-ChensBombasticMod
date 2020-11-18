using RoR2;
using RoR2.Artifacts;
using System.Collections.Generic;
using TILER2;
using UnityEngine;
using static RoR2.Artifacts.BombArtifactManager;

namespace Chen.Bombaholic
{
    public class Malice : Artifact_V2<Malice>
    {
        public override string displayName => "Artifact of Malice";

        protected override string GetNameString(string langid = null) => displayName;

        protected override string GetDescString(string langid = null) => "Enemies drop multiple exploding bombs on spawn.";

        public Malice()
        {
            iconResourcePath = "@ChensBombasticMod:Assets/Bombastic/texMaliceOn.png";
            iconResourcePathDisabled = "@ChensBombasticMod:Assets/Bombastic/texMaliceOff.png";
        }

        public override void Install()
        {
            base.Install();
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        public override void Uninstall()
        {
            base.Uninstall();
            CharacterBody.onBodyStartGlobal -= CharacterBody_onBodyStartGlobal;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
        }

        private void Run_onRunStartGlobal(Run obj)
        {
            if (IsActiveAndEnabled()) Run.instance.gameObject.AddComponent<MaliceManager>();
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody obj)
        {
            if (!IsActiveAndEnabled() || obj.teamComponent.teamIndex != TeamIndex.Monster) return;
            MaliceManager maliceManager = Run.instance.GetComponent<MaliceManager>();
            if (!maliceManager) return;

            Vector3 corePosition = obj.corePosition;
            float bombComputation = obj.bestFitRadius * extraBombPerRadius * cvSpiteBombCoefficient.value;
            int num = Mathf.Min(maxBombCount, Mathf.CeilToInt(bombComputation));
            for (int i = 0; i < num; i++)
            {
                Vector3 b = Random.insideUnitSphere * (bombSpawnBaseRadius + obj.bestFitRadius * bombSpawnRadiusCoefficient);
                BombRequest item = new BombRequest
                {
                    spawnPosition = corePosition,
                    raycastOrigin = corePosition + b,
                    bombBaseDamage = obj.damage * bombDamageCoefficient,
                    attacker = obj.gameObject,
                    teamIndex = obj.teamComponent.teamIndex,
                    velocityY = Random.Range(5f, 25f)
                };
                maliceManager.bombRequestQueue.Enqueue(item);
            }
        }
    }

    public class MaliceManager : MonoBehaviour
    {
        private Malice maliceInst;

        public readonly Queue<BombRequest> bombRequestQueue = new Queue<BombArtifactManager.BombRequest>();

        private void Awake()
        {
            maliceInst = Malice.instance;
        }

        private void FixedUpdate()
        {
            if (maliceInst.IsActiveAndEnabled() && bombRequestQueue.Count > 0)
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