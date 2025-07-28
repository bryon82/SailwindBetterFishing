using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class BF_FishingRodHolder : MonoBehaviour
    {

        private readonly Vector3 ROD_ROTATION_OFFSET = new Vector3(-40f, 180f, 0f);
        private readonly Vector3 LOCAL_ATTACH_OFFSET = new Vector3(0.309f, 1.1f, -0.38f);

        private Transform _itemRigidBody;
        private ShipItemFishingRod _attachedRod;

        private void Awake()
        {
            _itemRigidBody = transform.GetComponent<ShipItemLampHook>().itemRigidbodyC.transform;
        }

        internal void AttachRod(ShipItemFishingRod rod)
        {
            if (IsOccupied || rod == null)
                return;

            FishingRodHolders.Add(rod, this);
            _attachedRod = rod;

            Vector3 worldAttachPos = _itemRigidBody.TransformPoint(LOCAL_ATTACH_OFFSET);
            rod.itemRigidbodyC.transform.position = worldAttachPos;

            Quaternion worldAttachRotation = _itemRigidBody.rotation * Quaternion.Euler(ROD_ROTATION_OFFSET);
            rod.itemRigidbodyC.transform.rotation = worldAttachRotation;

            rod.itemRigidbodyC.attached = true;
        }

        internal void DetachRod()
        {
            if (_attachedRod == null)
                return;

            _attachedRod.itemRigidbodyC.attached = false;

            FishingRodHolders.Remove(_attachedRod);
            _attachedRod = null;
        }

        public bool IsOccupied => _attachedRod != null;
        public ShipItemFishingRod AttachedRod => _attachedRod;

        private void OnDestroy()
        {
            if (IsOccupied && _attachedRod != null)
            {
                DetachRod();
            }
        }
    }
}
