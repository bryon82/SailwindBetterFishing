//using BepInEx.Configuration;
//using HarmonyLib;
//using UnityEngine;
//using static BetterFishing.BF_Plugin;

//namespace BetterFishing
//{
//    // Heavily borrowed from NANDTweaks (https://github.com/NANDbrew/NANDTweaks).
//    // Only add if NANDTweaks does not.
//    internal class AddGoldAlbacorePatches
//    {
//        internal static Traverse NANDTweaks { get; set; }
//        internal static bool NTAlbacoreAreaEnabled => 
//            NANDTweaks != null &&  NANDTweaks.Field("albacoreArea").GetValue<ConfigEntry<bool>>().Value;
//        internal static GameObject fishesRegion;

//        [HarmonyAfter()]
//        [HarmonyPatch(typeof(OceanFishes), "Awake")]
//        internal static class FishCenterAdder
//        {   
//            public static void Postfix(ref LocalFishesRegion[] ___localFishesRegions)
//            {
//                if (NTAlbacoreAreaEnabled || fishesRegion != null)
//                    return;

//                fishesRegion = new GameObject(name: "albacore_center");
//                fishesRegion.transform.position = new Vector3(-36000f, 0f, -50000f);

//                LocalFishesRegion fishes = fishesRegion.AddComponent<LocalFishesRegion>();
//                fishes.overrideInfluence = 0.2f;
//                fishes.outerRadius = 7000;
//                fishes.innerRadius = 2000;
//                ___localFishesRegions = ___localFishesRegions.AddToArray(fishes);
//            }
//        }

//        [HarmonyPatch(typeof(PrefabsDirectory), "Start")]
//        internal static class FishPatch
//        {
//            public static void Postfix(GameObject[] ___directory)
//            {
//                if (NTAlbacoreAreaEnabled || fishesRegion == null)
//                    return;

//                fishesRegion.transform.parent = Refs.islands[4];
//                fishesRegion.GetComponent<LocalFishesRegion>().localFishPrefabs = new GameObject[] { ___directory[140] };
//                LogDebug($"fishCenter pos = {FloatingOriginManager.instance.GetGlobeCoords(fishesRegion.transform)}");
//            }
//        }
//    }
//}
