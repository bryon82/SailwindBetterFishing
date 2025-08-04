using BepInEx.Configuration;

namespace BetterFishing
{
    internal class Configs
    {
        internal static ConfigEntry<bool> enableFishMovement;
        internal static ConfigEntry<bool> enableFishTension;
        internal static ConfigEntry<int> hookLossChance;

        internal static void InitializeConfigs()
        {
            var config = BF_Plugin.Instance.Config;

            enableFishMovement = config.Bind(
                "Settings",
                "Enable fish movement",
                true,
                "Enables fish movement when fish caught. Fish will move around instead of just sitting still.");
            enableFishTension = config.Bind(
                "Settings",
                "Enable fish tension",
                true,
                "Enables fish tension. Some fish will cause more tension on the rod when reeling in.");
            hookLossChance = config.Bind(
                "Settings",
                "Fishing hook loss chance",
                31,
                new ConfigDescription("Precent chance that your fishing hook is lost after collecting a caught fish.", new AcceptableValueRange<int>(0, 100)));
        }
    }
}
