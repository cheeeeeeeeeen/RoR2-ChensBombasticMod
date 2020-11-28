#undef DEBUG

using BepInEx;
using BepInEx.Configuration;
using Chen.Helpers;
using Chen.Helpers.GeneralHelpers;
using Chen.Helpers.LogHelpers;
using R2API;
using R2API.Utils;
using TILER2;
using static Chen.Helpers.GeneralHelpers.AssetsManager;
using static TILER2.MiscUtil;
using Path = System.IO.Path;

namespace Chen.BombasticMod
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [BepInDependency(TILER2Plugin.ModGuid, TILER2Plugin.ModVer)]
    [BepInDependency(HelperPlugin.ModGuid, HelperPlugin.ModVer)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(nameof(ResourcesAPI))]
    sealed class BombasticPlugin : BaseUnityPlugin
    {
        public const string ModVer =
#if DEBUG
            "0." +
#endif
            "1.0.6";

        public const string ModName = "ChensBombasticMod";
        public const string ModGuid = "com.Chen.ChensBombasticMod";

        private static ConfigFile cfgFile;

        internal static FilingDictionary<CatalogBoilerplate> chensItemList = new FilingDictionary<CatalogBoilerplate>();

        internal static Log Log;

        private void Awake()
        {
            Log = new Log(Logger);

#if DEBUG
            MultiplayerTest.Enable(Logger, "Running test build with debug enabled! Report to CHEN if you're seeing this!");
#endif

            Log.Debug("Loading assets...");
            BundleInfo bundleInfo = new BundleInfo("@ChensBombasticMod", "ChensBombasticMod.chensbombasticmod_assets", BundleType.UnityAssetBundle);
            new AssetsManager(bundleInfo).RegisterAll();

            cfgFile = new ConfigFile(Path.Combine(Paths.ConfigPath, ModGuid + ".cfg"), true);

            Log.Debug("Instantiating item classes...");
            chensItemList = T2Module.InitAll<CatalogBoilerplate>(new T2Module.ModInfo
            {
                displayName = "Chen's Bombastic Mod",
                longIdentifier = "ChensBombasticMod",
                shortIdentifier = "CBM",
                mainConfigFile = cfgFile
            });

            T2Module.SetupAll_PluginAwake(chensItemList);
            T2Module.SetupAll_PluginStart(chensItemList);
        }

        private void Start()
        {
            CatalogBoilerplate.ConsoleDump(Logger, chensItemList);
        }
    }
}