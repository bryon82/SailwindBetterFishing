using System.Linq;
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

        public void SealCrate(ShipItemHammer hammer)
        {
            if (!crateInventory)
            {
                LogError($"{base.gameObject.name}: Cannot seal because no CrateInventory");
                return;
            }

            var containedItemIndex = crateInventory.containedItems[0].gameObject.GetComponent<SaveablePrefab>().prefabIndex;
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
            var countNeeded = FishData.Fish.Where(f => f.ItemName == inv[0].name).Select(f => f.NumberInCrate).FirstOrDefault();
            if (IsNotStandardCrateSize())
                sb.AppendLine("need standard size crate to seal");
            if (nailsNotInRange)
                sb.AppendLine("no sealing nails nearby");
            if (inv.Any(ci => ci.name != inv[0].name))
                sb.AppendLine("all items in crate must be the same to seal");
            else if (!FishData.SealableFish.Contains(inv[0].name))
                sb.AppendLine("items in crate are not sealable");
            if (countNeeded > 0 && inv.Count != countNeeded)
                sb.AppendLine($"not enough items in crate to seal {inv.Count}/{countNeeded}");
            if (inv.Any(ci => ci.GetComponent<FoodState>()?.spoiled > 0.9))
                sb.AppendLine("can not seal with spoiled items in the crate");
            if (inv.Any(ci => ci.amount >= 1.5))
                sb.AppendLine("can not seal with burnt items in the crate");
            if (inv.Any(ci => ci.GetComponent<FoodState>()?.smoked < 0.99 && ci.GetComponent<FoodState>()?.salted < 0.99 && ci.GetComponent<FoodState>()?.dried < 0.99))
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
            if (IsNotStandardCrateSize() || nailsNotInRange || inv.Count <= 0 || !FishData.SealableFish.Contains(inv[0].name) || inv.Count != FishData.Fish.Where(f => f.ItemName == inv[0].name).Select(f => f.NumberInCrate).FirstOrDefault() || inv.Any(ci => ci.name != inv[0].name || ci.amount >= 1.5 || ci.GetComponent<FoodState>()?.spoiled > 0.9 || (ci.GetComponent<FoodState>()?.smoked < 0.99 && ci.GetComponent<FoodState>()?.salted < 0.99 && ci.GetComponent<FoodState>()?.dried < 0.99)))
                return false;

            return true;
        }

        private bool IsNotStandardCrateSize()
        {
            var isEmptyOrStandardSize = crate.GetComponent<Good>()?.sizeDescription == "standard crate" || crate.name == "empty crate";
            return  !isEmptyOrStandardSize;
        }
    }
}
