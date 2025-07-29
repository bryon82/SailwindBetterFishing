using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class BF_ShipItemHolder : MonoBehaviour
    {

        private readonly Vector3 ROD_ROTATION_OFFSET = new Vector3(-40f, 180f, 0f);
        private readonly Vector3 ROD_LOCAL_ATTACH_OFFSET = new Vector3(0.309f, 1.1f, -0.38f);

        private readonly Vector3 BROOM_LOCAL_ATTACH_OFFSET = new Vector3(0f, -0.25f, -0.11f);

        private Transform _itemRigidBody;
        private ShipItem _attachedItem;

        private void Awake()
        {
            _itemRigidBody = transform.GetComponent<ShipItemLampHook>().itemRigidbodyC.transform;
        }

        internal void AttachItem(ShipItem item)
        {
            if (item is ShipItemFishingRod rod)
            {
                AttachRod(rod);
            }
            else if (item is ShipItemBroom broom)
            {
                AttachBroom(broom);
            }
        }

        private void AttachRod(ShipItemFishingRod rod)
        {
            if (IsOccupied || rod == null)
                return;

            ItemHolders.Add(rod, this);
            _attachedItem = rod;

            Vector3 worldAttachPos = _itemRigidBody.TransformPoint(ROD_LOCAL_ATTACH_OFFSET);
            rod.itemRigidbodyC.transform.position = worldAttachPos;

            Quaternion worldAttachRotation = _itemRigidBody.rotation * Quaternion.Euler(ROD_ROTATION_OFFSET);
            rod.itemRigidbodyC.transform.rotation = worldAttachRotation;

            rod.itemRigidbodyC.attached = true;
        }

        private void AttachBroom(ShipItemBroom broom)
        {
            if (IsOccupied || broom == null)
                return;

            ItemHolders.Add(broom, this);
            _attachedItem = broom;

            Vector3 worldAttachPos = _itemRigidBody.TransformPoint(BROOM_LOCAL_ATTACH_OFFSET);
            broom.itemRigidbodyC.transform.position = worldAttachPos;

            Quaternion worldAttachRotation = _itemRigidBody.rotation;
            broom.itemRigidbodyC.transform.rotation = worldAttachRotation;

            broom.itemRigidbodyC.attached = true;
        }

        internal void DetachItem()
        {
            if (_attachedItem == null)
                return;

            _attachedItem.itemRigidbodyC.attached = false;

            ItemHolders.Remove(_attachedItem);
            _attachedItem = null;
        }

        public bool IsOccupied => _attachedItem != null;
        public ShipItem AttachedItem => _attachedItem;

        private void OnDestroy()
        {
            if (IsOccupied)
            {
                DetachItem();
            }
        }
    }
}
