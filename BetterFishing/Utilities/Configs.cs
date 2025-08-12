using BepInEx.Configuration;

namespace BetterFishing
{
    internal class Configs
    {
        internal static ConfigEntry<bool> enableFishMovement;
        internal static ConfigEntry<bool> enableFishTension;
        internal static ConfigEntry<int> hookLossChance;
        internal static ConfigEntry<float> templeFishPriceMult;
        internal static ConfigEntry<float> sunspotFishPriceMult;
        internal static ConfigEntry<float> tunaPriceMult;        
        internal static ConfigEntry<float> shimmertailPriceMult;
        internal static ConfigEntry<float> salmonPriceMult;
        internal static ConfigEntry<float> eelPriceMult;
        internal static ConfigEntry<float> blackfinHunterPriceMult;
        internal static ConfigEntry<float> troutPriceMult;
        internal static ConfigEntry<float> northFishPriceMult;        
        internal static ConfigEntry<float> swampSnapperPriceMult;
        internal static ConfigEntry<float> blueBubblerPriceMult;
        internal static ConfigEntry<float> fireFishPriceMult;
        internal static ConfigEntry<float> goldAlbacorePriceMult;
        internal static ConfigEntry<bool> adjustStoveEfficiency;
        internal static ConfigEntry<bool> adjustSaltPrice;
        internal static ConfigEntry<bool> removeWholeFish;

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
            templeFishPriceMult = config.Bind(
                "Fish Price Multipliers",
                "templefish",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish individually or sealed.", new AcceptableValueRange<float>(0, 10)));            
            sunspotFishPriceMult = config.Bind(
                "Fish Price Multipliers",
                "sunspot fish",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish individually or sealed.", new AcceptableValueRange<float>(0, 10)));
            tunaPriceMult = config.Bind(
                "Fish Price Multipliers",
                "tuna",
                3f,
                new ConfigDescription("Price multiplier when buying or selling this fish.", new AcceptableValueRange<float>(0, 10)));
            shimmertailPriceMult = config.Bind(
                "Fish Price Multipliers",
                "blue shimmertail",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish individually or sealed.", new AcceptableValueRange<float>(0, 10)));
            salmonPriceMult = config.Bind(
                "Fish Price Multipliers",
                "salmon",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish individually or sealed.", new AcceptableValueRange<float>(0, 10)));
            eelPriceMult = config.Bind(
                "Fish Price Multipliers",
                "eel",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish.", new AcceptableValueRange<float>(0, 10)));
            blackfinHunterPriceMult = config.Bind(
                "Fish Price Multipliers",
                "blackfin hunter",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish individually or sealed.", new AcceptableValueRange<float>(0, 10)));
            troutPriceMult = config.Bind(
                "Fish Price Multipliers",
                "trout",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish individually or sealed.", new AcceptableValueRange<float>(0, 10)));
            northFishPriceMult = config.Bind(
                "Fish Price Multipliers",
                "north fish",
                3f,
                new ConfigDescription("Price multiplier when buying or selling this fish.", new AcceptableValueRange<float>(0, 10)));
            swampSnapperPriceMult = config.Bind(
                "Fish Price Multipliers",
                "swamp snapper",
                5f,
                new ConfigDescription("Price multiplier when buying or selling this fish.", new AcceptableValueRange<float>(0, 10)));
            blueBubblerPriceMult = config.Bind(
                "Fish Price Multipliers",
                "blue bubbler",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish.", new AcceptableValueRange<float>(0, 10)));
            fireFishPriceMult = config.Bind(
                "Fish Price Multipliers",
                "fire fish",
                2f,
                new ConfigDescription("Price multiplier when buying or selling this fish.", new AcceptableValueRange<float>(0, 10)));
            goldAlbacorePriceMult = config.Bind(
                "Fish Price Multipliers",
                "gold albacore",
                6f,
                new ConfigDescription("Price multiplier when buying or selling this fish.", new AcceptableValueRange<float>(0, 10)));
            adjustStoveEfficiency = config.Bind(
                "Settings",
                "Adjust stove efficiency",
                true,
                "This will make the stove and smoker cook faster. On the stove, fish will generally cook completely with one piece of firewood.");
            adjustSaltPrice = config.Bind(
                "Settings",
                "Adjust salt price",
                true,
                "This will make salt half the price.");
            removeWholeFish = config.Bind(
                "Settings",
                "Remove crateable whole fish from stores",
                true,
                "This will remove the whole fish that you are able to seal in a crate, from stores. In most instances they are replaced with fish slices.");
        }
    }
}
