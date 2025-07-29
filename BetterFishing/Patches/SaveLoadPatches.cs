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
        internal static List<BF_ShipItemHolder> SavedShipItemHolders { get; set; } = new List<BF_ShipItemHolder>();
        internal static List<ShipItem> SavedShipItems { get; set; } = new List<ShipItem>();

        [HarmonyPatch(typeof(SaveLoadManager))]
        private class SaveLoadManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("SaveModData")]
            public static void DoSaveGamePatch()
            {
                var saveContainer = new BetterFishingSaveContainer
                {
                    fishingRodHolders = ItemHolders.ToDictionary(
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
                    __instance.StartCoroutine(AttachItems(saveContainer.fishingRodHolders));
            }

            internal static IEnumerator AttachItems(Dictionary<int, int> holders)
            {
                yield return new WaitUntil(() => !GameState.currentlyLoading);

                SavedShipItems = SavedShipItems
                    .Where(item => item != null && item.GetComponent<SaveablePrefab>() != null)
                    .ToList();

                SavedShipItemHolders = SavedShipItemHolders
                    .Where(item => item != null && item.GetComponent<SaveablePrefab>() != null)
                    .ToList();

                foreach (var entry in holders)
                {
                    LogDebug($"Attaching item. Item ID: {entry.Key}, Holder ID: {entry.Value}");

                    var item = SavedShipItems.FirstOrDefault(r => r.GetComponent<SaveablePrefab>().instanceId == entry.Key);
                    var holder = SavedShipItemHolders.FirstOrDefault(h => h.GetComponent<SaveablePrefab>().instanceId == entry.Value);

                    if (item == null || holder == null)
                    {
                        LogWarning($"Failed to find item or holder for saved fishing rod holder. Item: {item} ID: {entry.Key}, Holder: {holder} ID: {entry.Value}.");
                        yield return null;
                    }

                    holder.AttachItem(item);
                }

                SavedShipItemHolders.Clear();
                SavedShipItems.Clear();
            }
        }
    }

    [Serializable]
    public class BetterFishingSaveContainer
    {
        internal Dictionary<int, int> fishingRodHolders;
    }
}
