using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterFishing
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(HOOKS_HANG_MORE_GUID, HOOKS_HANG_MORE_VERSION)]
    public class BF_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude.betterfishing";
        public const string PLUGIN_NAME = "BetterFishing";
        public const string PLUGIN_VERSION = "1.7.0";

        public const string HOOKS_HANG_MORE_GUID = "com.raddude.hookshangmore";
        public const string HOOKS_HANG_MORE_VERSION = "2.0.0";

        public const string SAILADEX_GUID = "com.raddude.sailadex";
        public const string ECONOMIC_EVENTS_GUID = "com.raddude.economicevents";
        public const string NANDTWEAKS_GUID = "com.nandbrew.nandtweaks";

        internal static BF_Plugin Instance { get; private set; }
        private static ManualLogSource _logger;

        internal static void LogDebug(string message) => _logger.LogDebug(message);
        internal static void LogInfo(string message) => _logger.LogInfo(message);
        internal static void LogWarning(string message) => _logger.LogWarning(message);
        internal static void LogError(string message) => _logger.LogError(message);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _logger = Logger;

            //foreach (var plugin in Chainloader.PluginInfos)
            //{
            //    var metadata = plugin.Value.Metadata;
            //    if (metadata.GUID.Equals(NANDTWEAKS_GUID))
            //    {
            //        LogInfo("NANDTweaks mod found");
            //        AddGoldAlbacorePatches.NANDTweaks = Traverse.Create(plugin.Value.Instance);
            //    }
            //}

            StartCoroutine(AssetLoader.LoadAssets());

            Configs.InitializeConfigs();
            FishData.Initialize();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
            SceneManager.sceneLoaded += AddShopItems.SceneLoaded;
            SceneManager.sceneLoaded += RemoveWholeFish.SceneLoaded;            
        }
    }
}
