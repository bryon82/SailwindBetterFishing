using System.Collections.Generic;
using static BetterFishing.Configs;

namespace BetterFishing
{
    internal sealed class FishData
    {
        internal string PrefabName { get; }
        internal string ItemName { get; }
        internal float Force { get; }
        internal float Tension { get; }
        internal int CrateIndex { get; }
        internal int ItemIndex { get; }
        internal float PriceMultiplier { get; }
        internal int NumberInCrate { get; }

        private FishData(string prefabName, string itemName, float force, float tension, int crateIndex, int itemIndex, float priceMultiplier, int numberInCrate)
        {
            PrefabName = prefabName;
            ItemName = itemName;
            Force = force;
            Tension = tension;
            CrateIndex = crateIndex;
            ItemIndex = itemIndex;
            PriceMultiplier = priceMultiplier;
            NumberInCrate = numberInCrate;
        }

        internal static IReadOnlyList<FishData> Fish
        {
            get
            {
                var fish = new List<FishData>
                {
                    new FishData("31 templefish (A)", "templefish", 10f, 0.95f, -1, 31, templeFishPriceMult.Value, -1),
                    new FishData("32 sunspot fish (A)", "sunspot fish", 13f, 0.85f, 9, 32, sunspotFishPriceMult.Value, 16),
                    new FishData("46 tuna (A)", "tuna", 20f, 0.78f, 6, 46, tunaPriceMult.Value, 16),
                    new FishData("35 shimmertail (E)", "blue shimmertail", 18f, 0.83f, -1, 35, shimmertailPriceMult.Value, -1),
                    new FishData("33 salmon (E)", "salmon", 26f, 0.77f, 1, 33, salmonPriceMult.Value, 12),
                    new FishData("34 eel (E)", "eel", 30f, 0.72f, 19, 34, eelPriceMult.Value, 9),
                    new FishData("38 blackfin hunter (M)", "blackfin hunter", 20f, 0.85f, -1, 38, blackfinHunterPriceMult.Value, -1),
                    new FishData("36 trout (M)", "trout", 25f, 0.74f, 18, 36, troutPriceMult.Value, 16),
                    new FishData("37 north fish (M)", "north fish", 22f, 0.79f, 14, 37, northFishPriceMult.Value, 16),
                    new FishData("141 swamp fish 1 (snapper", "swamp snapper", 21f, 0.83f, -1, 141, swampSnapperPriceMult.Value, -1),
                    new FishData("142 swamp fish 2 (bubbler)", "blue bubbler", 15f, 0.9f, -1, 142, blueBubblerPriceMult.Value, -1),
                    new FishData("148 swamp fish 3", "fire fish", 28f, 0.76f, -1, 148, fireFishPriceMult.Value, -1),
                    new FishData("140 gold albacore", "gold albacore", 32f, 0.7f, -1, 140, goldAlbacorePriceMult.Value, -1),
                };

                return fish.AsReadOnly();
            }
        }

        internal static IReadOnlyList<string> SealableFish
        {             
            get
            {
                var sealableFish = new List<string>();
                foreach (var fish in Fish)
                {
                    if (fish.CrateIndex > 0)
                    {
                        sealableFish.Add(fish.ItemName);
                    }
                }
                return sealableFish.AsReadOnly();
            }
        }
    }
}
