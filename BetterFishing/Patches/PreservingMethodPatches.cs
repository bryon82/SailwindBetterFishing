using HarmonyLib;
using static BetterFishing.Configs;

namespace BetterFishing
{
    internal class PreservingMethodPatches
    {
        [HarmonyPatch(typeof(ShipItem), "OnLoad")]
        private class ShipItemPatches
        {
            public static void Postfix(ShipItem __instance)
            {
                if (!adjustStoveEfficiency.Value)
                    return;

                if (__instance is ShipItemStove stove)
                {
                    if (stove.name == "stove")
                    {
                        var efficiency = stove.GetPrivateField<float>("cookEfficiency");
                    stove.SetPrivateField("cookEfficiency", efficiency * 1.303f);
                }
                    else if (stove.name == "smoker")
                    {
                        var efficiency = stove.GetPrivateField<float>("cookEfficiency");
                        stove.SetPrivateField("cookEfficiency", efficiency * 1.125f);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemSalt), "OnLoad")]
        private class ShipItemSaltPatches
        {
            public static void Postfix(ShipItemSalt __instance)
            {
                if (!adjustSaltPrice.Value)
                    return;

                __instance.value /= 2;
            }
        }
    }
}
