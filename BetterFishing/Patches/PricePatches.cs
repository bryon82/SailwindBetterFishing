using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterFishing.Patches
{
    internal class PricePatches
    {
        [HarmonyPatch(typeof(IslandMarket))]
        private class IslandMarketPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("GetGoodPriceAtSupply")]
            public static void FishPriceAdjust(int goodIndex, ref int __result)
            {
                var fish = FishData.Fish.FirstOrDefault(f => f.CrateIndex == goodIndex);
                if (fish != null)
                {
                    __result *= fish.PriceMultiplier;
                }
            }
        }

        [HarmonyPatch(typeof(Shopkeeper))]
        private class ShopkeeperPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("GetPrice")]
            public static void FishPriceAdjust(ShipItem item, ref int __result)
            {
                if (item.IsBulk())
                    return;

                var index = item.gameObject.GetComponent<SaveablePrefab>().prefabIndex;
                var fish = FishData.Fish.FirstOrDefault(f => f.CrateIndex == index || f.ItemIndex == index);
                if (fish != null)
                {
                    __result *= fish.PriceMultiplier;
                }
            }
        }
    }
}
