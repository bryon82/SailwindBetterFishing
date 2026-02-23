using System;
using HarmonyLib;
using UnityEngine;

namespace BetterFishing
{
    internal class PrefabLoadingPatches
    {
        const int NEW_PREFAB_DIR_SIZE = 808 + 1;

        [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
        internal class PrefabDirectoryPatches
        {
            public static void Prefix(PrefabsDirectory __instance)
            {
                if (__instance.directory.Length <= NEW_PREFAB_DIR_SIZE)
                    Array.Resize(ref __instance.directory, NEW_PREFAB_DIR_SIZE);

                var sharedMaterial = __instance.directory[1].GetComponent<MeshRenderer>().sharedMaterial;
                Items.EmptyCrate.GetComponent<MeshRenderer>().sharedMaterial = sharedMaterial;
                Items.SealingNails.GetComponent<MeshRenderer>().sharedMaterial = sharedMaterial;
                Items.CrateSpoonLures.GetComponent<MeshRenderer>().sharedMaterial = sharedMaterial;
                Items.CrateSwimbaitLures.GetComponent<MeshRenderer>().sharedMaterial = sharedMaterial;
                Items.CrateTopwaterLures.GetComponent<MeshRenderer>().sharedMaterial = sharedMaterial;

                LurePatches.hookMesh = __instance.directory[99].GetComponent<MeshFilter>().sharedMesh;
                LurePatches.hookMaterial = __instance.directory[99].GetComponent<MeshRenderer>().sharedMaterial;

                __instance.directory[800] = Items.EmptyCrate;
                __instance.directory[802] = Items.SealingNails;
                __instance.directory[803] = Items.SpoonLure;
                __instance.directory[804] = Items.SwimbaitLure;
                __instance.directory[805] = Items.TopwaterLure;
                __instance.directory[806] = Items.CrateSpoonLures;
                __instance.directory[807] = Items.CrateSwimbaitLures;
                __instance.directory[808] = Items.CrateTopwaterLures;
            }
        }
    }
}
