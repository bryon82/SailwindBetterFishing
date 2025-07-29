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
                    var holder = __instance.gameObject.AddComponent<BF_ShipItemHolder>();
                    SaveLoadPatches.SavedShipItemHolders.Add(holder);
                }
                if (__instance is ShipItemBroom broom)
                {                    
                    SaveLoadPatches.SavedShipItems.Add(broom);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ShipItem __instance, ref bool __result)
            {
                if (__instance is ShipItemFishingRod && (bool)lookedAtButton.GetComponent<BF_ShipItemHolder>())
                {
                    __result = true;
                    return false;
                }
                if (__instance is ShipItemBroom && (bool)lookedAtButton.GetComponent<BF_ShipItemHolder>())
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
                if ((__instance is ShipItemFishingRod fishingRod) && ItemHolders.ContainsKey(fishingRod))
                {
                    var holder = ItemHolders[fishingRod];
                    if (holder != null && holder.AttachedItem == fishingRod)
                    {
                        holder.DetachItem();
                    }
                }

                if ((__instance is ShipItemBroom broom) && ItemHolders.ContainsKey(broom))
                {
                    var holder = ItemHolders[broom];
                    if (holder != null && holder.AttachedItem == broom)
                    {
                        holder.DetachItem();
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
                SaveLoadPatches.SavedShipItems.Add(__instance);
            }
        }

        [HarmonyPatch(typeof(ShipItemLampHook))]
        private class ShipItemLampHookPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void OnPickup(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<BF_ShipItemHolder>();
                if (holder.IsOccupied)
                {
                    holder.DetachItem();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void OnEnterInventory(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<BF_ShipItemHolder>();
                if (holder.IsOccupied)
                {
                    holder.DetachItem();
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static bool OnItemClick(PickupableItem heldItem, ShipItemLampHook __instance, bool ___occupied, ref bool __result)
            {
                var holder = __instance.GetComponent<BF_ShipItemHolder>();
                if (holder.IsOccupied || ___occupied)
                {
                    __result = false;
                    return false;
                }

                var fishingRod = heldItem.GetComponent<ShipItemFishingRod>();
                if (fishingRod != null && fishingRod.sold)
                {
                    holder.AttachItem(fishingRod);
                    __result = true;
                    return false;
                }

                var broom = heldItem.GetComponent<ShipItemBroom>();
                if (broom != null && broom.sold)
                {
                    holder.AttachItem(broom);
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
                if (lampHook != null && (bool)___pointer.GetHeldItem() && lampHook.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(false);
                    ___showingIcon = false;
                    ___controlsText.text = "";
                }
                else if (lampHook != null && (bool)___pointer.GetHeldItem()?.GetComponent<ShipItemFishingRod>() && !lampHook.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "attach rod\n";
                }
                else if (lampHook != null && (bool)___pointer.GetHeldItem()?.GetComponent<ShipItemBroom>() && !lampHook.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "attach broom\n";
                }
            }
        }
    }
}
