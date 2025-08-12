using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace BetterFishing
{
    internal class PricePatches
    {
        [HarmonyPatch(typeof(IslandMarket))]
        private class IslandMarketPatches
        {
            [HarmonyBefore("com.raddude82.economicevents")]
            [HarmonyPostfix]
            [HarmonyPatch("GetGoodPriceAtSupply")]
            public static void FishPriceAdjust(int goodIndex, ref int __result)
            {
                var fish = FishData.Fish.FirstOrDefault(f => f.CrateIndex == goodIndex);
                if (fish != null)
                {
                    __result = Mathf.RoundToInt(__result * fish.PriceMultiplier);
                }
            }
        }

        [HarmonyPatch(typeof(Shopkeeper))]
        private class ShopkeeperPatches
        {
            [HarmonyBefore("com.raddude82.economicevents")]
            [HarmonyPostfix]
            [HarmonyPatch("GetPrice")]
            public static void FishPriceAdjust(ShipItem item, ref int __result)
            {
                if (item.IsBulk())
                    return;

                var index = item.GetPrefabIndex();
                var fish = FishData.Fish.FirstOrDefault(f => f.CrateIndex == index || f.ItemIndex == index || f.SliceIndex == index);
                if (fish != null)
                {
                    __result = Mathf.RoundToInt(__result * fish.PriceMultiplier);
                }
            }
        }
    }
}
