#undef DEBUG

using BepInEx;
using BepInEx.Configuration;
using Chen.Helpers;
using Chen.Helpers.GeneralHelpers;
using Chen.Helpers.LogHelpers;
using R2API.Utils;
using System.Runtime.CompilerServices;
using TILER2;
using UnityEngine;
using static Chen.Helpers.GeneralHelpers.AssetsManager;
using static TILER2.MiscUtil;
using Path = System.IO.Path;

[assembly: InternalsVisibleTo("ChensBombasticMod.Tests")]

namespace Chen.BombasticMod
{
    /// <summary>
    /// Unity plugin of the mod.
    /// </summary>
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [BepInDependency(TILER2Plugin.ModGuid, TILER2Plugin.ModVer)]
    [BepInDependency(HelperPlugin.ModGuid, HelperPlugin.ModVer)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class BombasticPlugin : BaseUnityPlugin
    {
        /// <summary>
        /// This mod's version.
        /// </summary>
        public const string ModVer =
#if DEBUG
            "0." +
#endif
            "2.0.1";

        /// <summary>
        /// This mod's name.
        /// </summary>
        public const string ModName = "ChensBombasticMod";

        /// <summary>
        /// This mod's GUID.
        /// </summary>
        public const string ModGuid = "com.Chen.ChensBombasticMod";

        private static ConfigFile cfgFile;

        internal static FilingDictionary<CatalogBoilerplate> chensItemList = new FilingDictionary<CatalogBoilerplate>();

        internal static Log Log;

        internal static AssetBundle assetBundle;

        private void Awake()
        {
            Log = new Log(Logger);

#if DEBUG
            MultiplayerTest.Enable(Logger, "Running test build with debug enabled! Report to CHEN if you're seeing this!");
#endif

            Log.Debug("Loading assets...");
            BundleInfo bundleInfo = new BundleInfo("ChensBombasticMod.chensbombasticmod_assets", BundleType.UnityAssetBundle);
            assetBundle = new AssetsManager(bundleInfo).Register() as AssetBundle;

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

        internal static bool DebugCheck()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}