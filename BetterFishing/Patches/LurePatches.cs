using HarmonyLib;
using System.Collections;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class LurePatches
    {
        internal static Material hookMaterial;
        internal static Mesh hookMesh;

        [HarmonyPatch(typeof(LookUI), "RegisterPointer")]
        private class GetGoPointer
        {
            public static void Prefix(GoPointer goPointer)
            {
                Go.Pointer = goPointer;
            }
        }        

        [HarmonyPatch(typeof(ShipItemFishingRod), "OnItemClick")]
        private class ShipItemFishingRodPatches
        {
            [HarmonyPrefix]
            public static bool AttachHookType(PickupableItem heldItem, ShipItemFishingRod __instance, GameObject ___hookVisuals, ref bool __result)
            {
                ShipItemFishingHook component = heldItem.GetComponent<ShipItemFishingHook>();
                if (component == null)
                {
                    __result = true;
                    return false;
                }

                ___hookVisuals.SetActive(true);

                if (__instance.health > 0f)
                {
                    if (___hookVisuals.name == "hook")
                    {
                        var prefab = PrefabsDirectory.instance.directory[99];
                        __instance.StartCoroutine(SwappingLures(prefab, heldItem));
                    }
                    else
                    {
                        var lure = Lure.Lures.FirstOrDefault(l => l.Name == ___hookVisuals.name);
                        __instance.StartCoroutine(SwappingLures(lure.Item, heldItem));
                    }
                }
                else
                {
                    component.GetComponent<ShipItem>().DestroyItem();
                }

                __instance.health = 1f;

                LogDebug($"Attaching {heldItem.name}");
                AttachLure(heldItem.name, ref ___hookVisuals);

                __result = true;
                return false;
            }
        }

        private static void AttachLure(string heldItemName, ref GameObject hookVisuals)
        {
            if (heldItemName == "99 fishing hook(Clone)")
            {
                hookVisuals.GetComponent<MeshFilter>().mesh = hookMesh;
                hookVisuals.GetComponent<MeshRenderer>().material = hookMaterial;
                hookVisuals.transform.localPosition = Vector3.zero;
                hookVisuals.name = "hook";
            }
            else
            {
                var lure = Lure.Lures.FirstOrDefault(l => $"{l.Name}(Clone)" == heldItemName);
                hookVisuals.GetComponent<MeshFilter>().mesh = lure.LureMesh;
                hookVisuals.GetComponent<MeshRenderer>().material = lure.LureMaterial;
                hookVisuals.transform.localPosition = lure.Offset;
                hookVisuals.name = lure.Name;
            }
        }

        private static IEnumerator SwappingLures(GameObject prefab, PickupableItem heldItem)
        {
            yield return new WaitForEndOfFrame();

            LogDebug($"Swapping lure {prefab.name} for {heldItem.name}");

            var heldPos = heldItem.transform.position;
            var heldRot = heldItem.transform.rotation;
            var heldParent = heldItem.transform.parent;

            heldItem.GetComponent<ShipItem>().DestroyItem();

            var lure = Object.Instantiate(prefab, heldPos, heldRot, heldParent);
            lure.GetComponent<SaveablePrefab>().RegisterToSave();
            var shipItem = lure.GetComponent<ShipItemFishingHook>();
            shipItem.sold = true;

            yield return new WaitForEndOfFrame();

            Go.Pointer.PickUpItem(shipItem);

            LogDebug("Swapped lures");            
        }

        [HarmonyPatch(typeof(SaveablePrefab))]
        private class SaveablePrefabPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("PrepareSaveData")]
            public static void SaveLure(ShipItem ___item, ref SavePrefabData __result)
            {
                if (!(___item is ShipItemFishingRod))                
                    return;

                var hookVisuals = ___item.GetComponent<ShipItemFishingRod>().GetPrivateField<GameObject>("hookVisuals");

                if (hookVisuals.name == "hook")
                {
                    __result.extraValue0 = 0;
                }
                else
                {
                    var lure = Lure.Lures.FirstOrDefault(l => l.Name == hookVisuals.name);
                    if (lure != null)                    
                        __result.extraValue0 = lure.SaveData;                    
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("Load")]
            public static void LoadLure(SavePrefabData data, SaveablePrefab __instance)
            {
                var shipItem = __instance.GetComponent<ShipItem>();

                if (shipItem != null && shipItem is ShipItemFishingRod rod)
                {
                    LogDebug($"Loading lure for fishing rod with saved data extraValue0: {data.extraValue0}");
                    var hookVisuals = rod.GetPrivateField<GameObject>("hookVisuals");
                    var attachedLure = "99 fishing hook";

                    if (data.extraValue0 > 0)
                    {
                        var lure = Lure.Lures.FirstOrDefault(l => l.SaveData == data.extraValue0);
                        if (lure != null)                        
                            attachedLure = lure.Name;                        
                    }

                    AttachLure($"{attachedLure}(Clone)", ref hookVisuals);
                }
            }
        }
    }
}
