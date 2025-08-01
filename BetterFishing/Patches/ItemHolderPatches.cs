﻿using HarmonyLib;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing.Patches
{
    internal class ItemHolderPatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadAddSavedFishingRodHolder(ShipItem __instance)
            {
                if (__instance is ShipItemLampHook)
                    __instance.gameObject.AddComponent<BF_ShipItemHolder>();

                if (__instance is ShipItemBroom broom)
                    broom.gameObject.AddComponent<BF_HolderAttachable>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ShipItem __instance, ref bool __result)
            {
                if (__instance.GetComponent<BF_HolderAttachable>() != null &&
                    lookedAtButton.GetComponent<BF_ShipItemHolder>() != null &&
                    !lookedAtButton.GetComponent<BF_ShipItemHolder>().IsOccupied)
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
                if (__instance.GetComponent<BF_HolderAttachable>() != null && AttachedItems.ContainsKey(__instance))
                {
                    var holder = AttachedItems[__instance];
                    if (holder != null && holder.AttachedItem == __instance)
                        holder.DetachItem();
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemFishingRod), "OnLoad")]
        private class ShipItemFishingRodPatches
        {
            public static void Postfix(ShipItemFishingRod __instance)
            {
                __instance.gameObject.AddComponent<BF_HolderAttachable>();
            }
        }

        [HarmonyPatch(typeof(ShipItemChipLog), "OnLoad")]
        private class ShipItemChipLogPatches
        {
            public static void Postfix(ShipItemChipLog __instance)
            {
                __instance.gameObject.AddComponent<BF_HolderAttachable>();
            }
        }

        [HarmonyPatch(typeof(ShipItemQuadrant), "OnLoad")]
        private class ShipItemQuadrantPatches
        {
            public static void Postfix(ShipItemQuadrant __instance)
            {
                __instance.gameObject.AddComponent<BF_HolderAttachable>();
            }
        }

        [HarmonyPatch(typeof(ShipItemKnife), "OnLoad")]
        private class ShipItemKnifePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddComponent(ShipItemKnife __instance)
            {
                __instance.gameObject.AddComponent<BF_HolderAttachable>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ref bool __result)
            {
                if ((bool)lookedAtButton.GetComponent<BF_ShipItemHolder>() && !lookedAtButton.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
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
                if (item.GetComponent<BF_HolderAttachable>() != null && item.sold)
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
            public static void ShowLookText(GoPointerButton button,TextMesh ___controlsText, GoPointer ___pointer, TextMesh ___textLicon, ref bool ___showingIcon)
            {
                var lampHook = button.GetComponent<ShipItemLampHook>();
                if (lampHook != null && (bool)___pointer.GetHeldItem() && lampHook.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(false);
                    ___showingIcon = false;
                    ___controlsText.text = "";
                }
                else if (lampHook != null && (bool)___pointer.GetHeldItem()?.GetComponent<BF_HolderAttachable>() && !lampHook.GetComponent<BF_ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = $"attach {___pointer.GetHeldItem()?.GetComponent<ShipItem>()?.name}\n";
                }
            }
        }
    }
}
