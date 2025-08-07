using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    public class Items
    {
        public static GameObject Hammer { get; internal set; }
        public static GameObject EmptyCrate { get; internal set; }
        public static GameObject SealingNails { get; internal set; }
        internal static GameObject NailsLabel { get; set; }

        internal static void InitializeHammer()
        {
            var itemHammer = Hammer.AddComponent<ShipItemHammer>();
            itemHammer.holdDistance = 0.875f;
            itemHammer.furniturePlaceHeight = 0.5f;
            itemHammer.heldRotationOffset = -50;
            itemHammer.mass = 1;
            itemHammer.value = 120;
            itemHammer.name = "hammer";
            itemHammer.category = TransactionCategory.toolsAndSupplies;
            itemHammer.inventoryScale = 1;
            itemHammer.inventoryRotation = 90;
            itemHammer.inventoryRotationX = -90;
            itemHammer.floaterHeight = 1.6f;
        }

        internal static void InitializeNailsOld()
        {
            var itemNails = SealingNails.AddComponent<ShipItemSealingNails>();
            itemNails.holdDistance = 1.25f;
            itemNails.furniturePlaceHeight = 0.15f;
            itemNails.heldRotationOffset = 180;
            itemNails.mass = 3;
            itemNails.value = 60;
            itemNails.name = "sealing nails";
            itemNails.category = TransactionCategory.toolsAndSupplies;
            itemNails.inventoryScale = 1;
            itemNails.inventoryRotation = 0;
            itemNails.inventoryRotationX = 0;
            itemNails.floaterHeight = 1.6f;
            itemNails.amount = 15;
            itemNails.allowPlacingItems = true;
            itemNails.big = true;
        }

        internal static void InitializeNails()
        {
            SealingNails.name = "sealing nails";
            Object.Destroy(SealingNails.GetComponent<ShipItemCrate>());
            var tracker = SealingNails.GetComponent("EmbarkTracker");
            if (tracker != null)
            {
                Object.Destroy(tracker);
            }

            SealingNails.GetComponent<SaveablePrefab>().prefabIndex = 802;

            var itemNails = SealingNails.AddComponent<ShipItemSealingNails>();
            itemNails.holdDistance = 1.25f;
            itemNails.furniturePlaceHeight = 0.15f;
            itemNails.heldRotationOffset = 180;
            itemNails.mass = 3;
            itemNails.value = 60;
            itemNails.name = "sealing nails";
            itemNails.category = TransactionCategory.toolsAndSupplies;
            itemNails.inventoryScale = 1;
            itemNails.inventoryRotation = 0;
            itemNails.inventoryRotationX = 0;
            itemNails.floaterHeight = 1.6f;
            itemNails.amount = 15;
            itemNails.allowPlacingItems = true;
            itemNails.big = true;

            NailsLabel.transform.parent = SealingNails.transform;
            NailsLabel.transform.localPosition = new Vector3(0.0f, 0.1252f, 0.0f);
            NailsLabel.transform.localRotation = Quaternion.Euler(90f, 22f, 0f);
            NailsLabel.transform.localScale = new Vector3(0.14f, 0.25f, 0.2f);
            NailsLabel.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 2999;

            Object.DontDestroyOnLoad(SealingNails);
            SealingNails.SetActive(false);

            LogDebug("Sealing nails initialized");
        }

        internal static void InitializeEmptyCrate()
        {
            EmptyCrate.name = "empty crate";
            Object.Destroy(EmptyCrate.GetComponent<Good>());
            var tracker = EmptyCrate.GetComponent("EmbarkTracker");
            if (tracker != null)
            {
                Object.Destroy(tracker);
            }

            EmptyCrate.GetComponent<SaveablePrefab>().prefabIndex = 800;

            var itemCrate = EmptyCrate.GetComponent<ShipItemCrate>();
            itemCrate.mass = 5;
            itemCrate.value = 45;
            itemCrate.name = "empty crate";
            itemCrate.category = TransactionCategory.toolsAndSupplies;
            itemCrate.amount = 0;

            Object.DontDestroyOnLoad(EmptyCrate);
            EmptyCrate.SetActive(false);

            LogDebug("Empty crate initialized");
        }
    }
}
