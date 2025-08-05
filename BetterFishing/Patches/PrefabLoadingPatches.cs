using System;
using HarmonyLib;

namespace BetterFishing
{
    internal class PrefabLoadingPatches
    {
        const int NEW_PREFAB_DIR_SIZE = 802 + 1;

        [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
        internal class PrefabDirectoryPatches
        {
            public static void Prefix(PrefabsDirectory __instance)
            {
                if (__instance.directory.Length <= NEW_PREFAB_DIR_SIZE)
                    Array.Resize(ref __instance.directory, NEW_PREFAB_DIR_SIZE);

                __instance.directory[800] = AssetLoader.EmptyCrate;
                __instance.directory[801] = AssetLoader.Hammer;
                __instance.directory[802] = AssetLoader.SealingNails;
            }
        }
    }
}
