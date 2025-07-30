using Crest;
using HarmonyLib;
using UnityEngine;
using static BetterFishing.Configs;
using static BetterFishing.BF_Plugin;

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

                var fishMovement = __instance.gameObject.AddComponent<BF_FishMovement>();
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
                    var maxTension = ___lastLineLength < 15f ? 0.95 : BF_FishMovement.FishTension(__instance.currentFish.name);
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

            [HarmonyBefore(new string[] { IDLE_FISHING_GUID })]
            [HarmonyPostfix]
            [HarmonyPatch("Update")]
            public static void Postfix(
                FishingRodFish __instance,
                ShipItemFishingRod ___rod,
                SimpleFloatingObject ___floater,
                ConfigurableJoint ___bobberJoint,
                ref float ___fishTimer)
            {
                if (__instance.currentFish != null ||
                    ___rod.health <= 0f ||
                    !AttachedItems.ContainsKey(___rod) ||
                    !___floater.InWater ||
                    ___bobberJoint.linearLimit.limit <= 1f ||
                    __instance.gameObject.layer == 16)
                {
                    return;
                }

                ___fishTimer -= Time.deltaTime;
                float value = Vector3.Distance(__instance.transform.position, ___rod.transform.position);
                float num = Mathf.InverseLerp(3f, 20f, value) * 2.5f + 0.5f;
                if (___fishTimer <= 0f)
                {
                    ___fishTimer = 1f;

                    num = IdleFishingFound ? num / 20f : num / 6.67f;
                    if (Random.Range(0f, 100f) < num)
                    {
                        __instance.CatchFish();
                    }
                }
            }
        }
    }
}
