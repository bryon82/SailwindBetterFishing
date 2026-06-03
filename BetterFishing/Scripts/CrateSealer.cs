using System.Text;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    public class CrateSealer : MonoBehaviour
    {
        ShipItemCrate crate;
        CrateInventory crateInventory;        

        private void Awake()
        {
            crate = GetComponent<ShipItemCrate>();
            crateInventory = GetComponent<CrateInventory>();
        }

        public void SealCrate(HammerCrateSealer hammer)
        {
            if (!crateInventory)
            {
                LogError($"{base.gameObject.name}: Cannot seal because no CrateInventory");
                return;
            }

            var containedItemIndex = crateInventory.containedItems[0].GetPrefabIndex();
            var num = crateInventory.containedItems.Count;
            for (int i = num - 1; i >= 0; i--)
            {
                LogDebug($"Removing item {i}");
                var item = crateInventory.containedItems[i];
                crateInventory.containedItems.Remove(item);
                item.GetItemRigidbody().attached = false;
                item.GetItemRigidbody().disableCol = false;
                item.GetItemRigidbody().inStove = false;
                item.GetComponent<SaveablePrefab>().Unregister();
                Object.Destroy(item.gameObject);
            }

            hammer.SwapCrate(containedItemIndex, transform.position, transform.rotation, transform.parent);
        }

        internal void UpdateDescription(bool nailsNotInRange)
        {
            crate.description = string.Empty;            

            if (crateInventory == null)
                return;

            var inv = crateInventory.containedItems;
            if (inv.Count <= 0)
                return;

            CratePatches.HintText.anchor = TextAnchor.UpperCenter;
            var sb = new StringBuilder();
            sb.AppendLine();
            var countNeeded = FishData.GetNumberInCrate(inv[0].name);
            if (IsWrongCrateSize())
                sb.AppendLine("need standard or small size crate to seal");
            if (nailsNotInRange)
                sb.AppendLine("no sealing nails nearby");
            var firstItemName = inv[0].name;
            var mismatchedItems = false;
            foreach (var item in inv)
            {
                if (item.name != firstItemName)
                {
                    mismatchedItems = true;
                    break;
                }
            }

            if (mismatchedItems)
                sb.AppendLine("all items in crate must be the same to seal");
            else if (!FishData.IsSealableFishName(firstItemName))
                sb.AppendLine("items in crate are not sealable");
            if (countNeeded > 0 && inv.Count != countNeeded)
                sb.AppendLine($"not enough items in crate to seal {inv.Count}/{countNeeded}");
            var hasSpoiled = false;
            var hasBurnt = false;
            var hasUnpreserved = false;
            foreach (var item in inv)
            {
                if (item.amount >= 1.5)
                {
                    hasBurnt = true;
                }

                var foodState = item.GetComponent<FoodState>();
                if (foodState != null)
                {
                    if (foodState.spoiled > 0.9)
                    {
                        hasSpoiled = true;
                    }

                    if (foodState.smoked < 0.99 && foodState.salted < 0.99 && foodState.dried < 0.99)
                    {
                        hasUnpreserved = true;
                    }
                }

                if (hasSpoiled && hasBurnt && hasUnpreserved)
                {
                    break;
                }
            }

            if (hasSpoiled)
                sb.AppendLine("can not seal with spoiled items in the crate");
            if (hasBurnt)
                sb.AppendLine("can not seal with burnt items in the crate");
            if (hasUnpreserved)
                sb.AppendLine("can not seal with non preserved items in the crate");
            
            crate.description = BuildDescription(sb.ToString());
        }

        private string BuildDescription(string desc)
        {
            const string red = "#4D0000";

            return $"<color={red}>{desc}</color>";
        }

        public bool CanSealCrate(bool nailsNotInRange)
        {
            var inv = crateInventory.containedItems;
            var firstItemName = inv.Count > 0 ? inv[0].name : null;
            var hasInvalidItem = false;
            if (firstItemName != null)
            {
                foreach (var item in inv)
                {
                    if (item.name != firstItemName || item.amount >= 1.5)
                    {
                        hasInvalidItem = true;
                        break;
                    }

                    var foodState = item.GetComponent<FoodState>();
                    if (foodState == null)
                    {
                        continue;
                    }

                    if (foodState.spoiled > 0.9 ||
                        (foodState.smoked < 0.99 &&
                        foodState.salted < 0.99 &&
                        foodState.dried < 0.99))
                    {
                        hasInvalidItem = true;
                        break;
                    }
                }
            }

            var canNotSeal =
                IsWrongCrateSize() ||
                nailsNotInRange ||
                inv.Count <= 0 ||
                firstItemName == null ||
                !FishData.IsSealableFishName(firstItemName) ||
                inv.Count != FishData.GetNumberInCrate(firstItemName) ||
                hasInvalidItem;

            if (canNotSeal)
                return false;

            return true;
        }

        private bool IsWrongCrateSize()
        {
            var sizeDescription = crate.GetComponent<Good>()?.sizeDescription;
            var isCorrectSize = 
                sizeDescription == "standard crate" || 
                sizeDescription == "small crate" || 
                crate.name == "empty crate" ||
                crate.name == "firewood" ||
                crate.name == "fishing hooks";
            return  !isCorrectSize;
        }
    }
}
