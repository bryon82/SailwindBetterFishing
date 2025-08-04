using HooksHangMore;
using System.Collections;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    public class ShipItemHammer : ShipItem
    {
        private bool animating;
        private bool swingingBack;
        private float initialHoldDistance;
        private float hammerTime;
        private ShipItemSealingNails closestNailsInRange;

        public override void OnLoad()
        {
            initialHoldDistance = holdDistance;
            animating = false;
            var attachable = gameObject.AddComponent<HolderAttachable>();
            attachable.PositionOffset = new Vector3(0.02f, -0.15f, -0.12f);
            attachable.RotationOffset = new Vector3(270f, 270f, 0f);
        }

        public override void OnAltActivate()
        {
            base.OnAltActivate();
            if (sold)
            {
                var pointedAtItem = held.GetPointedAtItem();
                if ((bool)pointedAtItem && pointedAtItem.sold && pointedAtItem.GetComponent<CrateSealer>() != null && pointedAtItem.GetComponent<CrateSealer>().CanSealCrate(!NailsInRange()))
                {
                    animating = true;
                    heldRotationOffset = 0f;
                    hammerTime = 0.25f;
                    SealCrate(pointedAtItem.GetComponent<ShipItemCrate>());
                }
            }
        }

        public override void ExtraLateUpdate()
        {
            if (hammerTime > 0f)
            {
                if (heldRotationOffset < -85f)
                {
                    swingingBack = true;
                }

                if (heldRotationOffset > 0f)
                {
                    swingingBack = false;
                }

                if (swingingBack)
                {
                    heldRotationOffset += Time.deltaTime * 550f;
                }
                else
                {
                    heldRotationOffset -= Time.deltaTime * 550f;
                }

                hammerTime -= Time.deltaTime;
            }
            else
                animating = false;

            if (held)
            {
                float radius = 2.5f;
                Vector3 hammerPosition = transform.position;

                Collider[] hits = Physics.OverlapSphere(hammerPosition, radius);

                Collider closest = null;
                float closestDistanceSqr = Mathf.Infinity;

                foreach (Collider hit in hits)
                {
                    var shipItemNails = hit.gameObject.GetComponent<ShipItemSealingNails>();
                    if ( shipItemNails == null || !shipItemNails.sold || shipItemNails.amount <= 0) continue;

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

        private bool NailsInRange()
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
        }

        public void SwapCrate(int itemIndex, Vector3 position, Quaternion rotation, Transform parent)
        {
            var crateIndex = FishData.Fish
                .Where(f => f.ItemIndex == itemIndex)
                .Select(f => f.CrateIndex)
                .FirstOrDefault();
            
            StartCoroutine(SwappingCrates(crateIndex, position, rotation, parent));
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

        public override bool AllowOnItemClick(GoPointerButton lookedAtButton)
        {
            lookedAtButton?.GetComponent<CrateSealer>()?.UpdateDescription(!NailsInRange());

            if ((bool)lookedAtButton.GetComponent<ShipItemHolder>() && !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
                return true;

            if (!big && lookedAtButton.allowPlacingItems)
                return true;

            return false;
        }
    }
}
