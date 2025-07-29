using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterFishing
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(IDLE_FISHING_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class BF_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.betterfishing";
        public const string PLUGIN_NAME = "BetterFishing";
        public const string PLUGIN_VERSION = "1.0.1";

        public const string MODSAVEBACKUPS_GUID = "com.raddude82.modsavebackups";
        public const string MODSAVEBACKUPS_VERSION = "1.1.1";

        public const string IDLE_FISHING_GUID = "com.isa_idlefishing.patch";

        internal static BF_Plugin Instance { get; private set; }
        private static ManualLogSource _logger;

        internal static void LogDebug(string message) => _logger.LogDebug(message);
        internal static void LogInfo(string message) => _logger.LogInfo(message);
        internal static void LogWarning(string message) => _logger.LogWarning(message);
        internal static void LogError(string message) => _logger.LogError(message);

        [SerializeField]
        internal static Dictionary<ShipItem, BF_ShipItemHolder> ItemHolders { get; set; }
        internal static bool IdleFishingFound { get; private set; } = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _logger = Logger;

            ItemHolders = new Dictionary<ShipItem, BF_ShipItemHolder>();

            Configs.InitializeConfigs();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);

            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID.Equals(IDLE_FISHING_GUID))
                {
                    LogInfo($"{IDLE_FISHING_GUID} found");
                    IdleFishingFound = true;
                }
            }
        }
    }
}
