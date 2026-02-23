using HarmonyLib;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class HammerPatches
    {
        [HarmonyPatch(typeof(ShipItem), "OnLoad")]
        private class ShipItemOnLoadPatches
        {
            public static void Postfix(ShipItem __instance)
            {
                
                if (__instance is ShipItemHammer)
                {                    
                    var hammerCrateSealer = __instance.gameObject.AddComponent<HammerCrateSealer>();
                    hammerCrateSealer.hammer = __instance.GetComponent<ShipItemHammer>();
                    hammerCrateSealer.initialHoldDistance = __instance.holdDistance;
                }
            }
        }

        [HarmonyPatch(typeof(LookUI), "showLookText")]
        private class LookUIShowLookTextPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ShowLookText")]
            public static void ShowLookText(
                GoPointerButton button,
                TextMesh ___controlsText,
                GoPointer ___pointer,
                TextMesh ___textLicon,
                TextMesh ___textRIcon,
                ref bool ___showingIcon)
            {
                var showCrateText =
                    (bool)button.GetComponent<ShipItemCrate>() &&
                    button.GetComponent<ShipItemCrate>().amount <= 0f &&
                    (bool)button.GetComponent<Good>() &&
                    button.GetComponent<Good>().GetMissionIndex() == -1 &&
                    (bool)___pointer.GetHeldItem() &&
                    ___pointer.GetHeldItem().GetComponent<HammerCrateSealer>();

                if (showCrateText)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___textRIcon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "place item\nseal crate";
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemHammer))]
        private class ShipItemHammerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnAltActivate")]
            public static void OnAltActivatePatch(ShipItemHammer __instance)
            {
                if (__instance.sold)
                {
                    var pointedAtItem = __instance.held.GetPointedAtItem();
                    if ((bool)pointedAtItem && pointedAtItem.sold &&
                        pointedAtItem.GetComponent<CrateSealer>() != null &&
                        pointedAtItem.GetComponent<CrateSealer>().CanSealCrate(!__instance.GetComponent<HammerCrateSealer>().NailsInRange()))
                    {
                        __instance.heldRotationOffset = 0f;
                        __instance.GetComponent<HammerCrateSealer>().hammerTime = 0.25f;
                        __instance.GetComponent<HammerCrateSealer>().SealCrate(pointedAtItem.GetComponent<ShipItemCrate>());
                    }
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static void AllowOnItemClickPatch(GoPointerButton lookedAtButton, ShipItemHammer __instance)
            {
                if (!__instance.sold)                
                    return;       

                lookedAtButton?.GetComponent<CrateSealer>()?.UpdateDescription(!__instance.GetComponent<HammerCrateSealer>().NailsInRange());
            }
        }
    }
}
