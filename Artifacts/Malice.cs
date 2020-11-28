using Chen.Helpers.UnityHelpers;
using RoR2;
using TILER2;
using UnityEngine.Networking;

namespace Chen.BombasticMod
{
    internal class Malice : Artifact_V2<Malice>
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
            if (NetworkServer.active && IsActiveAndEnabled()) obj.gameObject.GetOrAddComponent<BombasticManager>();
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody obj)
        {
            if (!NetworkServer.active || !IsActiveAndEnabled() || obj.teamComponent.teamIndex != TeamIndex.Monster) return;
            BombasticManager manager = Run.instance.GetComponent<BombasticManager>();
            if (!manager) return;

            manager.QueueBomb(obj);
        }
    }
}