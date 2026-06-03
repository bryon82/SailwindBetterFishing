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
        private FishingRodFish _fish;

        public FishingRodFish Fish
        {
            get => _fish;
            set => _fish = value;
        }

        private void Awake()
        {
            _timer = 0f;
            _fishForce = 0f;
        }

        private void Update()
        {
            if (GameState.wasInSettingsMenu)
                return;

            var fishDead = _fish.GetPrivateField<bool>("fishDead");
            var lastLineLength = _fish.GetPrivateField<float>("lastLineLength");
            var fishEnergy = _fish.GetPrivateField<float>("fishEnergy");
            var floater = _fish.GetPrivateField<SimpleFloatingObject>("floater");
            var bobber = _fish.GetPrivateField<Rigidbody>("bobber");

            fishDead = fishDead && lastLineLength <= 15f;
            _fish.SetPrivateField("fishDead", fishDead);

            if (fishDead || _fish.currentFish is null || fishEnergy <= 0f || !floater.InWater)
            {
                _fishForce = 0f;
                floater.SetPrivateField("_buoyancyCoeff", 3f);
                return;
            }

            if (_fishForce == 0f)
            {
                if (FishData.TryGetByPrefabName(_fish.currentFish.name, out var fish))
                {
                    _fishForce = fish.Force;
                }
                else
                {
                    _fishForce = 10f;
                    LogWarning($"{_fish.currentFish.name} not found");
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
            {
                return fish.Tension;
            }

            LogWarning($"{fishName} not found in fish data.");
            return 0.95f;
        }
    }
}
