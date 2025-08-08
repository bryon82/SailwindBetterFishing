//using HarmonyLib;
//using static BetterFishing.BF_Plugin;

//namespace BetterFishing.Patches
//{
//    internal class DebuggingPatches
//    {
//        [HarmonyPatch(typeof(SaveablePrefab))]
//        private class SaveablePrefabPatches
//        {
//            [HarmonyPrefix]
//            [HarmonyPatch("PrepareSaveData")]
//            public static void WhatItem(SaveablePrefab __instance)
//            { 
//                LogDebug($"Preparing save data for {__instance.name}");
//            }
//        }
//    }
//}
