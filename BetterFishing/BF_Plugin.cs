using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace BetterFishing
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(HOOKS_HANG_MORE_GUID, HOOKS_HANG_MORE_VERSION)]
    public class BF_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.betterfishing";
        public const string PLUGIN_NAME = "BetterFishing";
        public const string PLUGIN_VERSION = "1.1.6";

        public const string HOOKS_HANG_MORE_GUID = "com.raddude82.hookshangmore";
        public const string HOOKS_HANG_MORE_VERSION = "1.0.1";

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

            
            StartCoroutine(AssetLoader.LoadAssets());

            Configs.InitializeConfigs();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
            SceneManager.sceneLoaded += AddShopItems.SceneLoaded;
        }
    }
}
