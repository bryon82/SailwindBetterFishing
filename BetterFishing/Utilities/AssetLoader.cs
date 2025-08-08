using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class AssetLoader
    {
        private static readonly List<string> assetPaths = new List<string>() {
            Path.Combine(Path.GetDirectoryName(Instance.Info.Location), "Assets"),
            Path.Combine(Path.GetDirectoryName(Instance.Info.Location))
        };

        public static string FindAssetPath(string fileName)
        {
            foreach (string basePath in assetPaths)
            {
                string fullPath = Path.Combine(basePath, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }

        internal static IEnumerator LoadAssets()
        {
            LogDebug("Loading bundle");
            var bundlePath = FindAssetPath("tools_bundle");
            if (string.IsNullOrEmpty(bundlePath))
            {
                LogError("Asset bundle not found");
                yield break;
            }

            var assetBundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return assetBundleRequest;

            var assetBundle = assetBundleRequest.assetBundle;
            if (assetBundle == null)
                LogError($"Failed to load {bundlePath}");
            var request = assetBundle.LoadAllAssetsAsync();
            yield return request;

            Items.Hammer = request.allAssets.FirstOrDefault(a => a.name == "hammer") as GameObject;
            Items.EmptyCrate = request.allAssets.FirstOrDefault(a => a.name == "empty crate") as GameObject;
            Items.SealingNails = request.allAssets.FirstOrDefault(a => a.name == "sealing nails") as GameObject;

            if (Items.Hammer == null || Items.EmptyCrate == null || Items.SealingNails == null)
                LogError("Failed to load all assets from the bundle.");

            Items.InitializeHammer();
            Items.InitializeNails();
        }        
    }
}