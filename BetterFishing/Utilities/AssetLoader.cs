using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class AssetLoader
    {
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

            Items.Hammer = request.allAssets.FirstOrDefault(a => a.name == "hammer") as GameObject;
            Items.NailsLabel = request.allAssets.FirstOrDefault(a => a.name == "label") as GameObject;

            if (Items.Hammer == null || Items.NailsLabel == null)
                LogError("Failed to load all assets from the bundle.");

            Items.InitializeHammer();
        }        
    }
}