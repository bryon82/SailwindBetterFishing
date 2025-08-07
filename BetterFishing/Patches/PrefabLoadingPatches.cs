using System;
using HarmonyLib;
using Object = UnityEngine.Object;

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

                Items.EmptyCrate = Object.Instantiate(__instance.directory[1]);
                Items.InitializeEmptyCrate();

                Items.SealingNails = Object.Instantiate(__instance.directory[131]);                
                Items.InitializeNails();

                __instance.directory[800] = Items.EmptyCrate;
                __instance.directory[801] = Items.Hammer;
                __instance.directory[802] = Items.SealingNails;
            }
        }
    }
}
