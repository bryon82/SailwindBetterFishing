using Crest;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    public class FishMovement : MonoBehaviour
    {
        public readonly struct FishProperties
        {
            public float Force { get; }
            public float Tension { get; }

            public FishProperties(float force, float tension)
            {
                Force = force;
                Tension = tension;
            }
        }

        private float _timer;
        private float _fishForce;
        internal SimpleFloatingObject floater;
        internal Rigidbody bobber;

        private float _wiggleTime;
        private float _wiggleSeed;
        private float wiggleStrength = 5;

        internal FishingRodFish Fish { get; set; }

        private void Awake()
        {
            _timer = 0f;
            _fishForce = 0f;

            _wiggleSeed = Random.Range(0f, 1000f);
        }

        private void Update()
        {
            if (GameState.wasInSettingsMenu)
                return;

            if (bobber is null || floater is null)
            {
                floater = Fish.GetPrivateField<SimpleFloatingObject>("floater");
                bobber = Fish.GetPrivateField<Rigidbody>("bobber");
            }

            if (bobber is null || floater is null)
                return;

            UpdateFishWiggle(wiggleStrength);

            var fishDead = Fish.GetPrivateField<bool>("fishDead");
            var lastLineLength = Fish.GetPrivateField<float>("lastLineLength");
            var fishEnergy = Fish.GetPrivateField<float>("fishEnergy");

            fishDead = fishDead && lastLineLength <= 15f;
            Fish.SetPrivateField("fishDead", fishDead);

            if (fishDead || Fish.currentFish is null || fishEnergy <= 0f || !floater.InWater)
            {
                _fishForce = 0f;
                floater.SetPrivateField("_buoyancyCoeff", 3f);
                return;
            }

            if (_fishForce == 0f)
            {
                if (FishData.TryGetByPrefabName(Fish.currentFish.name, out var fish))
                {
                    _fishForce = fish.Force;
                }
                else
                {
                    _fishForce = 10f;
                    LogWarning($"{Fish.currentFish.name} not found");
                }
            }

            if (_timer <= 0f)
            {
                _fishForce = -_fishForce;
                _timer = 10f + Random.Range(0, 0.3f * _fishForce);
            }
            floater.SetPrivateField("_buoyancyCoeff", 1f);
            bobber.AddRelativeForce(Vector3.right * _fishForce);
            bobber.AddRelativeForce(Vector3.forward * Random.Range(0f, 5f));
            bobber.AddRelativeForce(Vector3.up * Random.Range(-3f, 0));

            _timer -= 2f * Time.deltaTime;
        }

        public static float FishTension(string fishName)
        {
            if (FishData.TryGetByPrefabName(fishName, out var fish))
                return fish.Tension;

            LogWarning($"{fishName} not found in fish data.");
            return 0.95f;
        }

        private void UpdateFishWiggle(float strength)
        {
            if (Fish.currentFish == null)
                return;

            _wiggleTime += Time.deltaTime;

            float t = _wiggleTime + _wiggleSeed;

            float sideForce =
                Mathf.Sin(t * 8.0f) * 0.08f +
                Mathf.Sin(t * 13.7f) * 0.03f;

            float forwardForce =
                Mathf.Sin(t * 6.5f) * 0.05f;

            float yawTorque =
                Mathf.Sin(t * 7.5f) * 2.5f +
                Mathf.Sin(t * 12.8f) * 1.0f;

            float rollTorque =
                Mathf.Sin(t * 9.2f) * 1.5f +
                Mathf.Sin(t * 15.1f) * 0.6f;

            bobber.AddRelativeForce(
                Vector3.right * sideForce * strength,
                ForceMode.Force
            );

            bobber.AddRelativeForce(
                Vector3.forward * forwardForce * strength,
                ForceMode.Force
            );

            bobber.AddRelativeTorque(
                Vector3.up * yawTorque * strength,
                ForceMode.Force
            );

            bobber.AddRelativeTorque(
                Vector3.forward * rollTorque * strength,
                ForceMode.Force
            );
        }
    }
}
