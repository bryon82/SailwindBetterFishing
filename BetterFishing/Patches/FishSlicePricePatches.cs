using HarmonyLib;

namespace BetterFishing.Patches
{
    internal class FishSlicePricePatches
    {
        [HarmonyPatch(typeof(ShipItemFood), "OnLoad")]
        private class ShipItemFoodPatches
        {
            public static void Postfix(ShipItemFood __instance)
            {
                if (__instance.name == "sunspot fish slice")
                {
                    __instance.value = 6;
                }
                else if (__instance.name == "tuna slice")
                {
                    __instance.value = 9;
                }
                else if (__instance.name == "templefish slice")
                {
                    __instance.value = 8;
                }
                else if (__instance.name == "salmon slice")
                {
                    __instance.value = 12;
                }
                else if (__instance.name == "eel slice")
                {
                    __instance.value = 23;
                }
                else if (__instance.name == "shimmertail slice")
                {
                    __instance.value = 9;
                }
                else if (__instance.name == "trout slice")
                {
                    __instance.value = 8;
                }
                else if (__instance.name == "north fish slice")
                {
                    __instance.value = 10;
                }
                else if (__instance.name == "blackfin slice")
                {
                    __instance.value = 9;
                }
            }
        }
    }
}
