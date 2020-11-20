using RoR2;
using TILER2;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.Artifacts.BombArtifactManager;

namespace Chen.BombasticMod
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
            if (NetworkServer.active && IsActiveAndEnabled()) BombasticManager.GetOrAddComponent(Run.instance);
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody obj)
        {
            if (!NetworkServer.active || !IsActiveAndEnabled() || obj.teamComponent.teamIndex != TeamIndex.Monster) return;
            BombasticManager manager = Run.instance.GetComponent<BombasticManager>();
            if (!manager) return;

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
                manager.bombRequestQueue.Enqueue(item);
            }
        }
    }
}