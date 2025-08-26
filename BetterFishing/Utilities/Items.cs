using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    public class Items
    {
        public static GameObject Hammer { get; internal set; }
        public static GameObject EmptyCrate { get; internal set; }
        public static GameObject SealingNails { get; internal set; }
        public static GameObject SpoonLure { get; internal set; }
        public static GameObject SwimbaitLure{ get; internal set; }
        public static GameObject TopwaterLure { get; internal set; }
        public static GameObject CrateSpoonLures { get; internal set; }
        public static GameObject CrateSwimbaitLures { get; internal set; }
        public static GameObject CrateTopwaterLures { get; internal set; }

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
            itemNails.heldRotationOffset = -45;
            itemNails.mass = 3;
            itemNails.value = 60;
            itemNails.name = "sealing nails";
            itemNails.category = TransactionCategory.toolsAndSupplies;
            itemNails.inventoryScale = 1;
            itemNails.inventoryRotation = 180;
            itemNails.inventoryRotationX = 270;
            itemNails.floaterHeight = 1.6f;
            itemNails.amount = 15;
            itemNails.allowPlacingItems = true;
            itemNails.big = false;
        }

        internal static void InitializeLure(GameObject lure, string desc)
        {
            var itemHook = lure.GetComponent<ShipItemFishingHook>();
            itemHook.description += $"\n{desc}";
            itemHook.value = 10;
        }

        internal static void Initialize()
        {
            InitializeHammer();
            InitializeNails();
            InitializeLure(SpoonLure, "good for catching tuna");
            InitializeLure(SwimbaitLure, "good for catching eel");
            InitializeLure(TopwaterLure, "good for catching north fish");
        }
    }
}
