using Crest;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;
using static BetterFishing.Configs;

namespace BetterFishing
{
    internal class FishingPatches
    {
        [HarmonyPatch(typeof(FishingRodFish))]
        private class FishingRodFishPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Awake")]
            public static void SetFishMovement(FishingRodFish __instance)
            {
                if (!enableFishMovement.Value)
                    return;

                var fishMovement = __instance.gameObject.AddComponent<FishMovement>();
                fishMovement.Fish = __instance;
            }

            [HarmonyPrefix]
            [HarmonyPatch("FixedUpdate")]
            public static bool FishingLineTension(
                FishingRodFish __instance,
                Transform ___rodRotator,
                ShipItemFishingRod ___rod,
                Rigidbody ___bobber,
                ConfigurableJoint ___bobberJoint,
                SimpleFloatingObject ___floater,
                AudioSource ___tensionAudio,
                float ___fishPullForce,
                ref float ___pullTensionMult,
                float ___lowerForceThreshold,
                float ___reelBendMult,
                ref float ___currentTargetTension,
                ref float ___lastLineLength,
                ref float ___currentFishForce,
                float ___angleBendMult,
                ref float ___pullForce,
                float ___fishEnergy,
                ref bool ___shakePong,
                ref float ___snapTimer)
            {
                if (!enableFishTension.Value)
                    return true;

                if (!___rod.sold)
                    return false;

                if (__instance.currentFish != null)
                {
                    if (___floater.InWater && !__instance.fishDead && ___fishEnergy > 0f)
                    {
                        ___currentFishForce = ___fishPullForce;
                    }
                    else
                    {
                        ___currentFishForce = 0f;
                    }

                    Vector3 normalized = (___rod.transform.position - ___bobber.transform.position).normalized;
                    ___bobber.AddForce(-normalized * Time.deltaTime * ___currentFishForce);
                    ___pullForce = Mathf.Lerp(___pullForce, ___bobberJoint.currentForce.magnitude, Time.deltaTime * 1.5f);
                    if (___pullForce >= ___lowerForceThreshold && ___currentTargetTension < 0.5f)
                    {
                        ___currentTargetTension += Time.deltaTime * ___pullTensionMult;
                        if (___currentTargetTension > 0.5f)
                        {
                            ___currentTargetTension = 0.5f;
                        }
                    }

                    if (___pullForce >= ___lowerForceThreshold)
                    {
                        ___currentTargetTension += (___lastLineLength - ___bobberJoint.linearLimit.limit) * ___reelBendMult;
                    }

                    if (___pullForce <= 0f || ___currentFishForce <= 0f || __instance.fishDead)
                    {
                        ___currentTargetTension -= Time.deltaTime * 1.25f;
                    }
                    else if (___currentTargetTension > 0.5f)
                    {
                        ___currentTargetTension -= Time.deltaTime * ___pullTensionMult * 0.2f;
                    }

                    //if (___currentTargetTension > 0.95f)
                    var maxTension = ___lastLineLength < 15f ? 0.95 : FishMovement.FishTension(__instance.currentFish.name);
                    if (___currentTargetTension > maxTension)
                    {
                        ___snapTimer += Time.deltaTime;
                        if (___shakePong)
                        {
                            ___rodRotator.Translate(Vector3.forward * 0.01f, Space.Self);
                        }
                        else
                        {
                            ___rodRotator.Translate(Vector3.back * 0.01f, Space.Self);
                        }

                        ___shakePong = !___shakePong;
                        if (!___tensionAudio.isPlaying)
                        {
                            ___tensionAudio.Play();
                        }

                        if (___snapTimer > 3.1f)
                        {
                            __instance.ReleaseFish();
                        }
                    }
                    else
                    {
                        ___snapTimer -= Time.deltaTime;
                        if (___snapTimer < 0f)
                        {
                            ___snapTimer = 0f;
                        }

                        if (___tensionAudio.isPlaying)
                        {
                            ___tensionAudio.Stop();
                        }
                    }
                }
                else
                {
                    ___currentTargetTension = 0f;
                    if (___tensionAudio.isPlaying)
                    {
                        ___tensionAudio.Stop();
                    }
                }

                if (___currentTargetTension > 1f)
                {
                    ___currentTargetTension = 1f;
                }

                if (___currentTargetTension < 0f)
                {
                    ___currentTargetTension = 0f;
                }

                ___rod.SetRodTension(___currentTargetTension * ___angleBendMult);
                ___lastLineLength = ___bobberJoint.linearLimit.limit;
                return false;
            }

            [HarmonyAfter(SAILADEX_GUID)]
            [HarmonyPrefix]
            [HarmonyPatch("CollectFish")]
            public static bool AdjustHookLossChance(FishingRodFish __instance, ShipItemFishingRod ___rod, ref ShipItem __result)
            {
                ShipItem component = Object.Instantiate(__instance.currentFish, __instance.gameObject.transform.position, __instance.gameObject.transform.rotation).GetComponent<ShipItem>();
                component.sold = true;
                component.GetComponent<SaveablePrefab>().RegisterToSave();
                __instance.GetComponent<MeshFilter>().sharedMesh = null;
                __instance.GetComponent<Renderer>().enabled = false;
                __instance.currentFish = null;
                if (Random.Range(0, 100) < hookLossChance.Value)
                {
                    ___rod.DetachHook();
                }

                ___rod.big = false;
                __result = component;
                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch("CatchFish")]
            public static bool CatchWithLure(
                FishingRodFish __instance,
                ref float ___fishTimer,
                ref float ___fishEnergy,
                Rigidbody ___bobber)
            {
                var lureName = ___bobber.transform.GetChild(2).name;
                if (lureName == "hook")
                    return true;

                var lure = Lure.Lures.FirstOrDefault(l => l.Name == lureName);
                var peakLatDiff = Mathf.Abs(FloatingOriginManager.instance.GetGlobeCoords(__instance.gameObject.transform).z - lure.PeakLatitude);
                if (peakLatDiff > 6)
                {
                    LogDebug($"{lure.Name} is not effective at this latitude");
                    return true;
                }

                int chance = 25;
                if (peakLatDiff <= 4)
                    chance = 50;
                if (peakLatDiff <= 2)
                    chance = 75;

                chance += lureEffectiveness.Value;

                LogDebug($"Lure {lure.Name} chance: {chance}");
                if (Random.Range(0, 100) > chance)
                {
                    LogDebug($"{lure.Name} caught other fish type");
                    return true;
                }

                LogInfo("============== Catching fish! ==============");
                __instance.currentFish = PrefabsDirectory.instance.directory[lure.TargetFishPrefabIndex];
                __instance.GetComponent<MeshFilter>().sharedMesh = __instance.currentFish.GetComponent<MeshFilter>().sharedMesh;
                __instance.GetComponent<Renderer>().enabled = true;
                ___fishTimer = 6f;
                ___fishEnergy = 1f;
                __instance.fishDead = false;

                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch("ReleaseFish")]
            public static bool DisableLineBreak()
            {
                return !disableLineBreak.Value;
            }
        }
    }
}
