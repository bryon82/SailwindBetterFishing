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

            Items.EmptyCrate = request.allAssets.FirstOrDefault(a => a.name == "empty crate") as GameObject;
            Items.SealingNails = request.allAssets.FirstOrDefault(a => a.name == "sealing nails") as GameObject;
            Items.SpoonLure = request.allAssets.FirstOrDefault(a => a.name == "spoon lure") as GameObject;
            Items.SwimbaitLure = request.allAssets.FirstOrDefault(a => a.name == "swimbait lure") as GameObject;
            Items.TopwaterLure = request.allAssets.FirstOrDefault(a => a.name == "topwater lure") as GameObject;
            Items.CrateSpoonLures = request.allAssets.FirstOrDefault(a => a.name == "crate of spoon lures") as GameObject;
            Items.CrateSwimbaitLures = request.allAssets.FirstOrDefault(a => a.name == "crate of swimbait lures") as GameObject;
            Items.CrateTopwaterLures = request.allAssets.FirstOrDefault(a => a.name == "crate of topwater lures") as GameObject;

            if (Items.EmptyCrate == null ||
                Items.SealingNails == null ||
                Items.SpoonLure == null ||
                Items.SwimbaitLure == null ||
                Items.TopwaterLure == null ||
                Items.CrateSpoonLures == null ||
                Items.CrateSwimbaitLures == null ||
                Items.CrateTopwaterLures == null)
            {
                LogError("Failed to load all required assets from the bundle");
                yield break;
            }

            LogInfo("Assets loaded");

            Items.Initialize();
        }
    }
}