using HarmonyLib;
using UnityEngine;

namespace BetterFishing
{
    internal class HammerPatches
    {
        [HarmonyPatch(typeof(LookUI))]
        private class LookUIPatches
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
                    (bool)___pointer.GetHeldItem()?.GetComponent<ShipItemHammer>();

                if (showCrateText)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___textRIcon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = "place item\nseal crate";
                }
            }
        }
    }
}
