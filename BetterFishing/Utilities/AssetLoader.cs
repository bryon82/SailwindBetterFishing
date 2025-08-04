using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class AssetLoader
    {
        public static GameObject Hammer { get; private set; }
        public static GameObject EmptyCrate { get; private set; }
        public static GameObject SealingNails { get; private set; }

        internal static IEnumerator LoadAssets()
        {
            LogDebug("Loading bundle");
            var bundlePath = Path.Combine(Path.GetDirectoryName(Instance.Info.Location), "Assets", "tools_bundle");
            var assetBundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return assetBundleRequest;

            var assetBundle = assetBundleRequest.assetBundle;
            if (assetBundle == null)
                LogError($"Failed to load {bundlePath}");
            var request = assetBundle.LoadAllAssetsAsync();
            yield return request;

            Hammer = request.allAssets.FirstOrDefault(a => a.name == "hammer") as GameObject;            
            EmptyCrate = request.allAssets.FirstOrDefault(a => a.name == "empty crate") as GameObject;
            SealingNails = request.allAssets.FirstOrDefault(a => a.name == "sealing nails") as GameObject;

            if (Hammer == null || EmptyCrate == null || SealingNails == null)
                LogError("Failed to load all assets from the bundle.");

            InitializeHammer();
            InitializeNails();
        }

        private static void InitializeHammer()
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

        private static void InitializeNails()
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