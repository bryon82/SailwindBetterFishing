using HarmonyLib;
using System.Linq;

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
                    __result *= fish.PriceMultiplier;
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
