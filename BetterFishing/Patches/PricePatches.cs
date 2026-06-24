using HarmonyLib;
using UnityEngine;

namespace BetterFishing
{
    internal class PricePatches
    {
        [HarmonyPatch(typeof(IslandMarket))]
        private class IslandMarketPatches
        {
            [HarmonyBefore("com.raddude.economicevents")]
            [HarmonyPostfix]
            [HarmonyPatch("GetGoodPriceAtSupply")]
            public static void FishPriceAdjust(int goodIndex, ref int __result)
            {
                if (FishData.TryGetByCrateIndex(goodIndex, out var fish))
                {
                    __result = Mathf.RoundToInt(__result * fish.PriceMultiplier);
                }
            }
        }

        [HarmonyPatch(typeof(Shopkeeper))]
        private class ShopkeeperPatches
        {
            [HarmonyBefore("com.raddude.economicevents")]
            [HarmonyPostfix]
            [HarmonyPatch("GetPrice")]
            public static void FishPriceAdjust(ShipItem item, ref int __result)
            {
                if (item.IsBulk())
                    return;

                var index = item.GetPrefabIndex();
                if (FishData.TryGetByAnyIndex(index, out var fish))
                {
                    __result = Mathf.RoundToInt(__result * fish.PriceMultiplier);
                }
            }
        }
    }
}
