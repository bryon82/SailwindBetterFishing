using HarmonyLib;
using ModSaveBackups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class SaveLoadPatches
    {
        internal static List<BF_FishingRodHolder> SavedFishingRodHolders { get; set; } = new List<BF_FishingRodHolder>();
        internal static List<ShipItemFishingRod> SavedShipItemFishingRods { get; set; } = new List<ShipItemFishingRod>();
        private static int _rodsAttached = 0;

        [HarmonyPatch(typeof(SaveLoadManager))]
        private class SaveLoadManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("SaveModData")]
            public static void DoSaveGamePatch()
            {
                var saveContainer = new BetterFishingSaveContainer
                {
                    fishingRodHolders = FishingRodHolders.ToDictionary(
                        entry => entry.Key.GetComponent<SaveablePrefab>().instanceId,
                        entry => entry.Value.GetComponent<SaveablePrefab>().instanceId
                    )
                };

                ModSave.Save(Instance.Info, saveContainer);
            }

            [HarmonyPostfix]
            [HarmonyPatch("LoadModData")]
            public static void LoadModDataPatch(SaveLoadManager __instance)
            {
                if (!ModSave.Load(Instance.Info, out BetterFishingSaveContainer saveContainer))
                {
                    LogWarning("Save file loading failed. If this is the first time loading this save with this mod, this is normal.");
                    return;
                }

                if (saveContainer.fishingRodHolders != null)
                {
                    foreach (var entry in saveContainer.fishingRodHolders)
                    {
                        __instance.StartCoroutine(AttachRods(entry, saveContainer.fishingRodHolders.Count));
                    }
                }
            }

            internal static IEnumerator AttachRods(KeyValuePair<int, int> entry, int holdersAttached)
            {
                yield return new WaitUntil(() => !GameState.currentlyLoading);
                var rod = SavedShipItemFishingRods.Where(r => r.GetComponent<SaveablePrefab>().instanceId == entry.Key).FirstOrDefault();
                var holder = SavedFishingRodHolders.Where(h => h.GetComponent<SaveablePrefab>().instanceId == entry.Value).FirstOrDefault();

                if (rod == null || holder == null)
                {
                    LogWarning($"Failed to find rod or holder for saved fishing rod holder. Rod: {rod} ID: {entry.Key}, Holder: {holder} ID: {entry.Value}.");
                    yield return null;
                }
                holder.AttachRod(rod);
                _rodsAttached++;

                if (_rodsAttached == holdersAttached)
                {
                    LogDebug($"All rods attached: {_rodsAttached}/{holdersAttached}");
                    SavedFishingRodHolders.Clear();
                    SavedShipItemFishingRods.Clear();
                }
            }
        }
    }

    [Serializable]
    public class BetterFishingSaveContainer
    {
        internal Dictionary<int, int> fishingRodHolders;
    }
}
