using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BetterFishing.BF_Plugin;
using static BetterFishing.Configs;

namespace BetterFishing
{
    internal class RemoveWholeFish
    {
        internal static void SceneLoaded(Scene scene, LoadSceneMode _)
        {
            if (!removeWholeFish.Value)
                return;

            if (scene.name == "island 1 A Gold Rock")
                PlaceSlices("island 1 A (gold rock) scenery");
            
            if (scene.name == "island 2 A AlNilem")
                PlaceSlices("island 2 A (alnilem) scenery");
            
            if (scene.name == "island 3 A Neverdin")
                PlaceSlices("island 3 A (neverdin) scenery");
            
            if (scene.name == "island 4 A Albacore Town")
                AlbacoreTown("island 4 A (fish island) scenery");

            if (scene.name == "island 9 E Dragon Cliffs")
                PlaceSlices("island 9 E (dragon cliffs) scenery");

            if (scene.name == "island 10 E (Sanctuary)")
                PlaceSlices("island 10 E (sanctuary) scenery");

            if (scene.name == "island 11 E (Crab Beach)")
                PlaceSlices("island 11 E (crab beach) scenery");

            if (scene.name == "island 12 E (New Port)")
                NewPort("island 12 E (new port) scenery");

            if (scene.name == "island 13 E (Sage Hills)")
                PlaceSlices("island 13 E (sage hills) scenery");

            if (scene.name == "island 15 M (Fort)")
                PlaceSlices("island 15 M (Fort) scenery");

            if (scene.name == "island 16 M (Sunspire)")
                PlaceSlices("island 16 M (sunspire) scenery");

            if (scene.name == "island 17 M (Mount Malefic)")
                PlaceSlices("island 17 M (mount malefic) scenery");

            if (scene.name == "island 18 M (Oasis)")
                PlaceSlices("island 18 M (Oasis) scenery");

            if (scene.name == "island 19 M (Eastwind)")
                Eastwind("island 19 M (Eastwind) scenery");

            if (scene.name == "island 20 A (Oasis)")
                Oasis("island 20 A (oasis) scenery");

            if (scene.name == "island 21 M (siren song)")
                PlaceSlices("island 21 M (siren song) scenery");

            if (scene.name == "island 22 E (Serpent Isle)")
                PlaceSlices("island 22 E (serpent isle) scenery");

            if (scene.name == "island 25 (chronos)")
                PlaceSlices("island 25 (chronos) scenery");

            if (scene.name == "island 26 Lagoon SwampTemple")
                PlaceSlices("island lagoon Temple scenery");

            if (scene.name == "island 27 Lagoon SwampShipyard")
                PlaceSlices("island scenery Lagoon Shipyard");

            if (scene.name == "island 33 M (cave)")
                PlaceSlices("scenery");
        }

        internal static void PlaceSlices(string sceneObject)
        {
            var scenery = GameObject.Find(sceneObject);
            if (scenery == null)
            {
                LogError($"Scenery not found: {sceneObject}.");
                return;
            }

            foreach (var fish in FishData.SealableFish)
            {
                var spawners = scenery.GetComponentsInChildren<ShopItemSpawner>().Where(s => s.itemPrefab.name == fish.PrefabName);
                foreach (var spawner in spawners)
                {
                    LogDebug($"Changing {fish.ItemName} spawner: {spawner.name}");
                    spawner.itemPrefab = PrefabsDirectory.instance.directory[fish.SliceIndex];
                }
            }            
        }

        internal static void AlbacoreTown(string sceneObject)
        {
            var scenery = GameObject.Find(sceneObject);
            if (scenery == null)
            {
                LogError($"Scenery not found: {sceneObject}.");
                return;
            }            
            
            var spawners = scenery.GetComponentsInChildren<ShopItemSpawner>().Where(s => s.itemPrefab.name == "46 tuna (A)");
            foreach (var spawner in spawners)
            {
                LogDebug($"Changing tuna spawner: {spawner.name}");
                spawner.itemPrefab = PrefabsDirectory.instance.directory[140];
            }            
        }

        internal static void NewPort(string sceneObject)
        {
            var scenery = GameObject.Find(sceneObject);
            if (scenery == null)
            {
                LogError($"Scenery not found: {sceneObject}.");
                return;
            }
            
            var spawnerNames = new[] { "shop item (5)", "shop item (6)", "shop item (9)" };
            
            foreach (var spawnerName in spawnerNames)
            {
                var spawner = scenery.GetComponentsInChildren<ShopItemSpawner>().FirstOrDefault(s => s.name == spawnerName);
                spawner.itemPrefab = PrefabsDirectory.instance.directory[31];
                spawner.transform.localPosition = new Vector3(spawner.transform.localPosition.x, 5.214f, spawner.transform.localPosition.z);
            }

            PlaceSlices(sceneObject);
        }

        internal static void Eastwind(string sceneObject)
        {
            var scenery = GameObject.Find(sceneObject);
            if (scenery == null)
            {
                LogError($"Scenery not found: {sceneObject}.");
                return;
            }

            var spawnerNames = new[] { "shop item (3)", "shop item (4)", "shop item (5)", "shop item (6)" };

            foreach (var spawnerName in spawnerNames)
            {
                var spawner = scenery.GetComponentsInChildren<ShopItemSpawner>().FirstOrDefault(s => s.name == spawnerName);
                spawner.itemPrefab = PrefabsDirectory.instance.directory[38];
                spawner.transform.localPosition = new Vector3(spawner.transform.localPosition.x, 3.383f, spawner.transform.localPosition.z);
            }

            PlaceSlices(sceneObject);
        }

        internal static void Oasis(string sceneObject)
        {
            var scenery = GameObject.Find(sceneObject);
            if (scenery == null)
            {
                LogError($"Scenery not found: {sceneObject}.");
                return;
            }

            var spawners = scenery.GetComponentsInChildren<ShopItemSpawner>().Where(s => s.itemPrefab.name == "46 tuna (A)");
            foreach (var spawner in spawners)
            {
                LogDebug($"Changing tuna spawner: {spawner.name}");
                spawner.itemPrefab = PrefabsDirectory.instance.directory[31];
                spawner.transform.localPosition = new Vector3(spawner.transform.localPosition.x, 16.36f, spawner.transform.localPosition.z);
            }

            PlaceSlices(sceneObject);
        }
    }
}
