using HarmonyLib;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class CratePatches
    {
        private static GoPointer _goPointer;
        internal static TextMesh HintText {  get; private set; }

        [HarmonyPatch(typeof(LookUI), "RegisterPointer")]
        private class LookUIPatches
        {
            public static void Prefix(GoPointer goPointer, TextMesh ___hintText)
            {
                _goPointer = goPointer;

                if (HintText == null)
                {
                    HintText = ___hintText;
                }
            }
        }


        [HarmonyPatch(typeof(ShipItemCrate))]
        private class ShipItemCratePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddCrateSealer(ShipItemCrate __instance)
            {
                __instance.gameObject.AddComponent<CrateSealer>();

                if (__instance.name == "empty crate")
                    __instance.value = 45;
            }

            [HarmonyPostfix]
            [HarmonyPatch("UpdateLookText")]
            public static void ClearDescription(ShipItemCrate __instance)
            {
                if (!GameState.currentlyLoading && GameState.playing && __instance.sold && !(_goPointer.GetHeldItem() is ShipItemHammer))
                {
                    __instance.description = string.Empty;
                    HintText.anchor = TextAnchor.MiddleCenter;
                }                    
            }
        }

        [HarmonyPatch(typeof(CrateInventoryUI))]
        private class CrateInventoryUIPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("GetCrateDimensions")]
            public static bool GetCrateDimensionsPatch(CrateInventoryUI __instance, ref Vector2 __result)
            {
                if (__instance.currentCrate.name == "empty crate(Clone)")
                {
                    __result = new Vector2(5f, 4f);
                    return false;

                }
                return true;   
            }
        }
    }
}
