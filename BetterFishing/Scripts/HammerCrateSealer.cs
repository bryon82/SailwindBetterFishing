using System.Collections;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    public class HammerCrateSealer : MonoBehaviour
    {
        private bool swingingBack;
        internal float initialHoldDistance;
        internal float hammerTime;
        private ShipItemSealingNails closestNailsInRange;
        internal ShipItemHammer hammer;

        public void Update()
        {
            if (hammerTime > 0f)
            {
                if (hammer.heldRotationOffset < -85f)
                {
                    swingingBack = true;
                }

                if (hammer.heldRotationOffset > 0f)
                {
                    swingingBack = false;
                }

                if (swingingBack)
                {
                    hammer.heldRotationOffset += Time.deltaTime * 550f;
                }
                else
                {
                    hammer.heldRotationOffset -= Time.deltaTime * 550f;
                }

                hammerTime -= Time.deltaTime;
            }

            if (hammer.held)
            {
                float radius = 2.5f;
                Vector3 hammerPosition = transform.position;

                Collider[] hits = Physics.OverlapSphere(hammerPosition, radius);

                Collider closest = null;
                float closestDistanceSqr = Mathf.Infinity;

                foreach (Collider hit in hits)
                {
                    var shipItemNails = hit.gameObject.GetComponent<ShipItemSealingNails>();
                    if (shipItemNails == null || !shipItemNails.sold || shipItemNails.amount <= 0) continue;

                    float distanceSqr = (hit.transform.position - hammerPosition).sqrMagnitude;
                    if (distanceSqr < closestDistanceSqr)
                    {
                        closest = hit;
                        closestDistanceSqr = distanceSqr;
                    }
                }

                closestNailsInRange = closest?.gameObject?.GetComponent<ShipItemSealingNails>();
            }
        }

        internal bool NailsInRange()
        {
            if (closestNailsInRange == null)
                return false;

            return closestNailsInRange.amount > 0;
        }

        public void SealCrate(ShipItemCrate crate)
        {
            closestNailsInRange.amount--;
            crate.GetComponent<CrateSealer>().SealCrate(this);
            crate.GetComponent<SaveablePrefab>().Unregister();
            Destroy(crate.gameObject);
            UISoundPlayer.instance.PlaySmallItemDropSound();
        }

        public void SwapCrate(int itemIndex, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (FishData.TryGetByItemIndex(itemIndex, out var fish))
            {
                StartCoroutine(SwappingCrates(fish.CrateIndex, position, rotation, parent));
            }
        }

        private IEnumerator SwappingCrates(int crateIndex, Vector3 position, Quaternion rotation, Transform parent)
        {
            yield return new WaitForEndOfFrame();

            var prefab = PrefabsDirectory.instance.GetItem(crateIndex);
            var crate = Instantiate(prefab, position, rotation, parent);
            crate.GetComponent<SaveablePrefab>().RegisterToSave();
            crate.sold = true;
            crate.GetComponent<Good>().RegisterAsMissionless();
            LogDebug("Swapped crates");
        }
    }
}
