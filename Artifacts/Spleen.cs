using Chen.Helpers.UnityHelpers;
using RoR2;
using TILER2;
using UnityEngine.Networking;

namespace Chen.BombasticMod
{
    internal class Spleen : Artifact_V2<Spleen>
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
            if (NetworkServer.active && IsActiveAndEnabled()) obj.gameObject.GetOrAddComponent<BombasticManager>();
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            if (!NetworkServer.active || !IsActiveAndEnabled()) return;
            CharacterBody body = self.gameObject.GetComponent<CharacterBody>();
            if (body.teamComponent.teamIndex != TeamIndex.Monster) return;
            if (!Util.CheckRoll(throwBombChance, body.master)) return;
            BombasticManager manager = Run.instance.GetComponent<BombasticManager>();
            if (!manager) return;

            manager.QueueBomb(body);
        }
    }
}