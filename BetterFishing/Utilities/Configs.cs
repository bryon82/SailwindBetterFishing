using BepInEx.Configuration;

namespace BetterFishing
{
    internal class Configs
    {
        internal static ConfigEntry<bool> enableFishMovement;
        internal static ConfigEntry<bool> enableFishTension;

        internal static void InitializeConfigs()
        {
            var config = BF_Plugin.Instance.Config;

            enableFishMovement = config.Bind("Settings", "Enable fish movement", true, "Enables fish movement when fish caught. Fish will move around instead of just sitting still.");
            enableFishTension = config.Bind("Settings", "Enable fish tension", true, "Enables fish tension. Some fish will cause more tension on the rod when reeling in.");            
        }
    }
}
