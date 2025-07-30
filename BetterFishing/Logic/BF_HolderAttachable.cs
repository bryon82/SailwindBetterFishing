using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class BF_HolderAttachable : MonoBehaviour
    {
        private ShipItem _shipItem;
        private BF_ShipItemHolder _shipItemHolder;
        private bool _disallowHangingOnTrigger;
        private float _framesAfterAwake;

        private void Awake()
        {
            _shipItem = GetComponent<ShipItem>();
            _shipItemHolder = null;
            _disallowHangingOnTrigger = false;
            _framesAfterAwake = 0f;
        }

        private void FixedUpdate()
        {
            if (_framesAfterAwake < 3f)
                _framesAfterAwake += 1f;
        }

        public void LoadInInventory()
        {
            _disallowHangingOnTrigger = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!(_framesAfterAwake >= 3f) && !_shipItem.held && _shipItem.GetCurrentInventorySlot() == -1 && !_disallowHangingOnTrigger && other.CompareTag("Hook") && !other.GetComponent<BF_ShipItemHolder>().IsOccupied)
            {
                var holder = other.GetComponent<BF_ShipItemHolder>();
                holder.AttachItem(_shipItem);
                _shipItemHolder = holder;
            }
        }

        //public void OnTriggerExit(Collider other)
        //{
        //    if (other.CompareTag("Hook") && _shipItemHolder != null && other.GetComponent<BF_ShipItemHolder>() == _shipItemHolder)
        //        _shipItemHolder.DetachItem();
        //}

        public void DetachHolder()
        {
            if (_shipItemHolder != null)
                _shipItemHolder = null;
        }

        public void OnDestroy()
        {
            _shipItemHolder?.DetachItem();
            _disallowHangingOnTrigger = false;
        }
    }
}
