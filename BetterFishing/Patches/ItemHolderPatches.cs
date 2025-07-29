using HarmonyLib;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing.Patches
{
    internal class ItemHolderPatches
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
                if ((__instance is ShipItemFishingRod || __instance is ShipItemBroom || __instance is ShipItemChipLog || __instance is ShipItemQuadrant) && (bool)lookedAtButton.GetComponent<BF_ShipItemHolder>())
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
                if ((__instance is ShipItemFishingRod || __instance is ShipItemBroom || __instance is ShipItemChipLog || __instance is ShipItemQuadrant) && ItemHolders.ContainsKey(__instance))
                {
                    var holder = ItemHolders[__instance];
                    if (holder != null && holder.AttachedItem == __instance)
                    {
                        holder.DetachItem();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemFishingRod), "OnLoad")]
        private class ShipItemFishingRodPatches
        {
            public static void Postfix(ShipItemFishingRod __instance)
            {
                SaveLoadPatches.SavedShipItems.Add(__instance);
            }
        }

        [HarmonyPatch(typeof(ShipItemChipLog), "OnLoad")]
        private class ShipItemChipLogPatches
        {
            public static void Postfix(ShipItemChipLog __instance)
            {
                SaveLoadPatches.SavedShipItems.Add(__instance);
            }
        }

        [HarmonyPatch(typeof(ShipItemQuadrant), "OnLoad")]
        private class ShipItemQuadrantPatches
        {
            public static void Postfix(ShipItemQuadrant __instance)
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

                var item = heldItem.GetComponent<ShipItem>();
                if ((item is ShipItemFishingRod || item is ShipItemBroom || item is ShipItemChipLog || item is ShipItemQuadrant) && item.sold)
                {
                    holder.AttachItem(item);
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
                else if (lampHook != null && (bool)___pointer.GetHeldItem()?.GetComponent<ShipItemChipLog>() && !lampHook.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "attach chip log\n";
                }
                else if (lampHook != null && (bool)___pointer.GetHeldItem()?.GetComponent<ShipItemQuadrant>() && !lampHook.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "attach quadrant\n";
                }
            }
        }
    }
}
