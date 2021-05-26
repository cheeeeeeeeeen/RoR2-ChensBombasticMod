using Chen.Helpers.UnityHelpers;
using RoR2;
using TILER2;
using UnityEngine;
using UnityEngine.Networking;
using static Chen.BombasticMod.BombasticPlugin;

namespace Chen.BombasticMod
{
    internal class Spleen : Artifact<Spleen>
    {
        public override string displayName => "Artifact of Spleen";

        [AutoConfig("Percentage chance of enemies throwing bombs when attacked. 5 means 5% chance.", AutoConfigFlags.None, 0f, 100f)]
        public float throwBombChance { get; private set; } = 15f;

        [AutoConfig("The limit of the bomb generation queue. 0 means that there's no limit. " +
                    "Any positive number means that the queue will be limited to contain the specified number of requests. Any procs will be disregarded if the limit is reached.",
                    AutoConfigFlags.None, 0, int.MaxValue)]
        public int spleenQueueLimit { get; private set; } = 0;

        [AutoConfig("The processing interval of the bomb generation queue in seconds. For example, if 2 is specified, a bomb will be generated per two seconds. " +
                    "No need to modify unless too much bombs cause lag. In return, higher interval means that the queue may have more requests than it can process, which can cause a crash.",
                    AutoConfigFlags.None, 0f, 10f)]
        public float spleenQueueProcessingInterval { get; private set; } = 0f;

        protected override string GetNameString(string langid = null) => displayName;

        protected override string GetDescString(string langid = null) => "Enemies have a chance to drop multiple exploding bombs when attacked.";

        public Spleen()
        {
            iconResource = assetBundle.LoadAsset<Sprite>("Assets/Bombastic/texSpleenOn.png");
            iconResourceDisabled = assetBundle.LoadAsset<Sprite>("Assets/Bombastic/texSpleenOff.png");
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
            if (NetworkServer.active && IsActiveAndEnabled()) obj.GetOrAddComponent<SpleenManager>();
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            if (!NetworkServer.active || !IsActiveAndEnabled()) return;
            CharacterBody body = self.gameObject.GetComponent<CharacterBody>();
            if (body.teamComponent.teamIndex != TeamIndex.Monster) return;
            if (!Util.CheckRoll(throwBombChance, body.master)) return;
            SpleenManager manager = Run.instance.GetComponent<SpleenManager>();
            if (!manager) return;

            manager.QueueBomb(body);
        }
    }
}