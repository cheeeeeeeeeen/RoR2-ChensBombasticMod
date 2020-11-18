using RoR2;
using TILER2;
using UnityEngine;
using static RoR2.Artifacts.BombArtifactManager;

namespace Chen.BombasticMod
{
    public class Spleen : Artifact_V2<Spleen>
    {
        public override string displayName => "Artifact of Spleen";

        [AutoConfig("Percentage chance of enemies throwing bombs when attacked. 5 means 5% chance.", AutoConfigFlags.None, 0f, 100f)]
        public float throwBombChance { get; private set; } = 15f;

        protected override string GetNameString(string langid = null) => displayName;

        protected override string GetDescString(string langid = null) => "Enemies have a chance to drop multiple exploding bombs when attacked.";

        public Spleen()
        {
            iconResourcePath = "@ChensBombasticMod:Assets/Bombastic/texSpleenOn.png";
            iconResourcePathDisabled = "@ChensBombasticMod:Assets/Bombastic/texSpleenOff.png";
        }

        public override void Install()
        {
            base.Install();
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        public override void Uninstall()
        {
            base.Uninstall();
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
        }

        private void Run_onRunStartGlobal(Run obj)
        {
            if (IsActiveAndEnabled()) BombasticManager.GetOrAddComponent(Run.instance);
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            CharacterBody body = self.gameObject.GetComponent<CharacterBody>();
            if (!IsActiveAndEnabled() || body.teamComponent.teamIndex != TeamIndex.Monster) return;
            if (!Util.CheckRoll(throwBombChance, body.master)) return;
            BombasticManager manager = Run.instance.GetComponent<BombasticManager>();
            if (!manager) return;

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
                manager.bombRequestQueue.Enqueue(item);
            }
        }
    }
}