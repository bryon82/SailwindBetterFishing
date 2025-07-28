using HarmonyLib;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing.Patches
{
    internal class RodHolderPatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemFishingHookPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadAddSavedFishingRodHolder(ShipItem __instance)
            {
                if (__instance is ShipItemLampHook)
                {
                    var holder = __instance.gameObject.AddComponent<BF_FishingRodHolder>();
                    SaveLoadPatches.SavedFishingRodHolders.Add(holder);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ShipItem __instance, ref bool __result)
            {
                if (__instance is ShipItemFishingRod && (bool)lookedAtButton.GetComponent<BF_FishingRodHolder>())
                {
                    __result = true;
                    return false;
                }

                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnPickup")]
            public static void OnShipItemPickup(ShipItem __instance)
            {
                if ((__instance is ShipItemFishingRod fishingRod) && FishingRodHolders.ContainsKey(fishingRod))
                {
                    var holder = FishingRodHolders[fishingRod];
                    if (holder != null && holder.AttachedRod == fishingRod)
                    {
                        holder.DetachRod();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemFishingRod))]
        private class ShipItemFishingRodPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadAddSavedFishingRod(ShipItemFishingRod __instance)
            {
                SaveLoadPatches.SavedShipItemFishingRods.Add(__instance);
            }
        }

        [HarmonyPatch(typeof(ShipItemLampHook))]
        private class ShipItemLampHookPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void OnPickup(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<BF_FishingRodHolder>();
                if (holder.IsOccupied && holder.AttachedRod != null)
                {
                    holder.DetachRod();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void OnEnterInventory(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<BF_FishingRodHolder>();
                if (holder.IsOccupied && holder.AttachedRod != null)
                {
                    holder.DetachRod();
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static bool OnItemClick(PickupableItem heldItem, ShipItemLampHook __instance, bool ___occupied, ref bool __result)
            {
                var holder = __instance.GetComponent<BF_FishingRodHolder>();
                if (holder.IsOccupied || ___occupied)
                {
                    __result = false;
                    return false;
                }

                var fishingRod = heldItem.GetComponent<ShipItemFishingRod>();
                if (fishingRod != null && fishingRod.sold)
                {
                    holder.AttachRod(fishingRod);
                    __result = true;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(LookUI))]
        private class LookUIPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ShowLookText")]
            public static void ShowLookText(GoPointerButton button, LookUI __instance, TextMesh ___controlsText, GoPointer ___pointer, TextMesh ___textLicon, ref bool ___showingIcon)
            {
                var lampHook = button.GetComponent<ShipItemLampHook>();
                if (lampHook != null && (bool)___pointer.GetHeldItem() && lampHook.GetComponent<BF_FishingRodHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(false);
                    ___showingIcon = false;
                    ___controlsText.text = "";
                }
                else if (lampHook != null && (bool)___pointer.GetHeldItem()?.GetComponent<ShipItemFishingRod>() && !lampHook.GetComponent<BF_FishingRodHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "attach rod\n";
                }
            }
        }
    }
}
