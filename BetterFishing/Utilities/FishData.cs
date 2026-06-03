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
        internal int SliceIndex { get; }
        internal float PriceMultiplier { get; }
        internal int NumberInCrate { get; }

        private FishData(string prefabName, string itemName, float force, float tension, int crateIndex, int itemIndex, int sliceIndex, float priceMultiplier, int numberInCrate)
        {
            PrefabName = prefabName;
            ItemName = itemName;
            Force = force;
            Tension = tension;
            CrateIndex = crateIndex;
            ItemIndex = itemIndex;
            SliceIndex = sliceIndex;
            PriceMultiplier = priceMultiplier;
            NumberInCrate = numberInCrate;
        }

        private static HashSet<string> _sealableFishNameSet;
        private static Dictionary<string, FishData> _fishByPrefabName;
        private static Dictionary<string, FishData> _fishByItemName;
        private static Dictionary<int, FishData> _fishByCrateIndex;
        private static Dictionary<int, FishData> _fishByItemIndex;
        private static Dictionary<int, FishData> _fishBySliceIndex;

        internal static IReadOnlyList<FishData> Fish { get; private set; }
        internal static IReadOnlyList<string> SealableFishNames { get; private set; }
        internal static IReadOnlyList<FishData> SealableFish {  get; private set; }

        internal static bool IsSealableFishName(string name)
        {
            return _sealableFishNameSet.Contains(name);
        }

        internal static bool TryGetByPrefabName(string name, out FishData fish)
        {
            return _fishByPrefabName.TryGetValue(name, out fish);
        }

        internal static bool TryGetByItemName(string name, out FishData fish)
        {
            return _fishByItemName.TryGetValue(name, out fish);
        }

        internal static bool TryGetByCrateIndex(int index, out FishData fish)
        {
            return _fishByCrateIndex.TryGetValue(index, out fish);
        }

        internal static bool TryGetByItemIndex(int index, out FishData fish)
        {
            return _fishByItemIndex.TryGetValue(index, out fish);
        }

        internal static bool TryGetBySliceIndex(int index, out FishData fish)
        {
            return _fishBySliceIndex.TryGetValue(index, out fish);
        }

        internal static bool TryGetByAnyIndex(int index, out FishData fish)
        {
            return _fishByCrateIndex.TryGetValue(index, out fish) ||
                   _fishByItemIndex.TryGetValue(index, out fish) ||
                   _fishBySliceIndex.TryGetValue(index, out fish);
        }

        internal static int GetNumberInCrate(string itemName)
        {
            return TryGetByItemName(itemName, out var fish) ? fish.NumberInCrate : 0;
        }

        internal static void Initialize()
        {
            if (Fish != null)
                return;

            var fish = new List<FishData>
            {
                new FishData("31 templefish (A)", "templefish", 10f, 0.95f, -1, 31, 351, templeFishPriceMult.Value, -1),
                new FishData("32 sunspot fish (A)", "sunspot fish", 13f, 0.85f, 9, 32, 352, sunspotFishPriceMult.Value, 16),
                new FishData("46 tuna (A)", "tuna", 20f, 0.78f, 6, 46, 366, tunaPriceMult.Value, 16),
                new FishData("35 shimmertail (E)", "blue shimmertail", 18f, 0.83f, -1, 35, 355, shimmertailPriceMult.Value, -1),
                new FishData("33 salmon (E)", "salmon", 26f, 0.77f, 1, 33, 45, salmonPriceMult.Value, 12),
                new FishData("34 eel (E)", "eel", 30f, 0.72f, 19, 34, 354, eelPriceMult.Value, 9),
                new FishData("38 blackfin hunter (M)", "blackfin hunter", 20f, 0.85f, -1, 38, 358, blackfinHunterPriceMult.Value, -1),
                new FishData("36 trout (M)", "trout", 25f, 0.74f, 18, 36, 356, troutPriceMult.Value, 16),
                new FishData("37 north fish (M)", "north fish", 22f, 0.79f, 14, 37, 357, northFishPriceMult.Value, 16),
                new FishData("141 swamp fish 1 (snapper", "swamp snapper", 21f, 0.83f, -1, 141, 353, swampSnapperPriceMult.Value, -1),
                new FishData("142 swamp fish 2 (bubbler)", "blue bubbler", 15f, 0.9f, -1, 142, 362, blueBubblerPriceMult.Value, -1),
                new FishData("148 swamp fish 3", "fire fish", 28f, 0.76f, -1, 148, 365, fireFishPriceMult.Value, -1),
                new FishData("140 gold albacore", "gold albacore", 32f, 0.7f, -1, 140, 348, goldAlbacorePriceMult.Value, -1),
            };

            Fish = fish.AsReadOnly();
            SealableFish = fish.FindAll(item => item.CrateIndex > 0).AsReadOnly();
            var sealableNames = new List<string>(SealableFish.Count);
            foreach (var item in SealableFish)
            {
                sealableNames.Add(item.ItemName);
            }

            SealableFishNames = sealableNames.AsReadOnly();
            _sealableFishNameSet = new HashSet<string>(sealableNames);

            _fishByPrefabName = new Dictionary<string, FishData>();
            _fishByItemName = new Dictionary<string, FishData>();
            _fishByCrateIndex = new Dictionary<int, FishData>();
            _fishByItemIndex = new Dictionary<int, FishData>();
            _fishBySliceIndex = new Dictionary<int, FishData>();

            foreach (var item in fish)
            {
                if (!string.IsNullOrEmpty(item.PrefabName))
                {
                    _fishByPrefabName[item.PrefabName] = item;
                }

                if (!string.IsNullOrEmpty(item.ItemName))
                {
                    _fishByItemName[item.ItemName] = item;
                }

                if (item.CrateIndex > 0)
                {
                    _fishByCrateIndex[item.CrateIndex] = item;
                }

                if (item.ItemIndex > 0)
                {
                    _fishByItemIndex[item.ItemIndex] = item;
                }

                if (item.SliceIndex > 0)
                {
                    _fishBySliceIndex[item.SliceIndex] = item;
                }
            }
        }
    }
}
