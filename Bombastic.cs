#undef DEBUG

using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using System.Reflection;
using TILER2;
using UnityEngine;
using static TILER2.MiscUtil;
using Path = System.IO.Path;

namespace Chen.BombasticMod
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [BepInDependency(TILER2Plugin.ModGuid, TILER2Plugin.ModVer)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(nameof(ResourcesAPI))]
    public class BombasticPlugin : BaseUnityPlugin
    {
        public const string ModVer =
#if DEBUG
            "0." +
#endif
            "1.0.4";

        public const string ModName = "ChensBombasticMod";
        public const string ModGuid = "com.Chen.ChensBombasticMod";

        private static ConfigFile cfgFile;

        internal static FilingDictionary<CatalogBoilerplate> chensItemList = new FilingDictionary<CatalogBoilerplate>();

        internal static BepInEx.Logging.ManualLogSource _logger;

        private void Awake()
        {
            _logger = Logger;
            
#if DEBUG
            Log.Warning("Running test build with debug enabled! Report to CHEN if you're seeing this!");
            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (self, user, t) => { };
#endif

            Log.Debug("Loading assets...");
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChensBombasticMod.chensbombasticmod_assets"))
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider("@ChensBombasticMod", bundle);
                ResourcesAPI.AddProvider(provider);
            }

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

    public static class Log
    {
        public static void Debug(object data) => logger.LogDebug(data);

        public static void Error(object data) => logger.LogError(data);

        public static void Info(object data) => logger.LogInfo(data);

        public static void Message(object data) => logger.LogMessage(data);

        public static void Warning(object data) => logger.LogWarning(data);

        public static BepInEx.Logging.ManualLogSource logger => BombasticPlugin._logger;
    }
}