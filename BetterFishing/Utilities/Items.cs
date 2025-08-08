using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    public class Items
    {
        public static GameObject Hammer { get; internal set; }
        public static GameObject EmptyCrate { get; internal set; }
        public static GameObject SealingNails { get; internal set; }

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

        internal static void InitializeNails()
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
    }
}
