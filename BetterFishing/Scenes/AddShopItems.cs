using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BetterFishing.BF_Plugin;

namespace BetterFishing
{
    internal class AddShopItems
    {
        internal static void SceneLoaded(Scene scene, LoadSceneMode _)
        {
            if (scene.name == "island 1 A Gold Rock")
                GoldRockCity();
            if (scene.name == "island 15 M (Fort)")
                FortAestrin();
            if (scene.name == "island 9 E Dragon Cliffs")
                DragonCliffs();
            if (scene.name == "island 26 Lagoon SwampTemple")
                FireFishTown();
        }

        internal static void GoldRockCity()
        {
            var scenery = GameObject.Find("island 1 A (gold rock) scenery");
            if (scenery == null)
            {
                LogError("Gold Rock City scenery not found.");
                return;
            }            

            MakeShopItem("shop item 300", scenery.transform, new Vector3(1539.7f, 7.235f, -385.26f), new Vector3(358f, 0f, 270f), AssetLoader.Hammer);
            MakeShopItem("shop item 301", scenery.transform, new Vector3(1538.75f, 5.71f, -385.08f), new Vector3(0f, 55f, 0f), AssetLoader.SealingNails);
            MakeShopItem("shop item 302", scenery.transform, new Vector3(1538.75f, 6.17f, -385.08f), new Vector3(0f, 55f, 0f), AssetLoader.SealingNails);
            MakeShopItem("shop item 303", scenery.transform, new Vector3(1542.949f, 5.54f, -389.34f), new Vector3(0f, 325.504f, 0f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item 304", scenery.transform, new Vector3(1535.669f, 6.54f, -378.58f), new Vector3(90f, 325.504f, 0f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item 305", scenery.transform, new Vector3(1537.369f, 5.5f, -381.38f), new Vector3(0f, 325.504f, 0f), AssetLoader.EmptyCrate);
        }

        internal static void FortAestrin()
        {
            var scenery = GameObject.Find("island 15 M (Fort) scenery");
            if (scenery == null)
            {
                LogError("Fort Aestrin scenery not found.");
                return;
            }

            MakeShopItem("shop item (300)", scenery.transform, new Vector3(-76.959f, 3.08f, 44.419f), new Vector3(358f, 80f, 270f), AssetLoader.Hammer);
            MakeShopItem("shop item (301)", scenery.transform, new Vector3(-75.854f, 2.21f, 44.5095f), new Vector3(0f, 180f, 0f), AssetLoader.SealingNails);
            MakeShopItem("shop item (302)", scenery.transform, new Vector3(-75.854f, 2.44f, 44.5095f), new Vector3(0f, 180f, 0f), AssetLoader.SealingNails);
            MakeShopItem("shop item (303)", scenery.transform, new Vector3(-78.403f, 2.097f, 43.976f), new Vector3(0f, 90f, 0f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item (304)", scenery.transform, new Vector3(-78.403f, 2.097f, 42.69f), new Vector3(0f, 90f, 0f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item (305)", scenery.transform, new Vector3(-78.403f, 2.783f, 43.333f), new Vector3(0f, 90f, 0f), AssetLoader.EmptyCrate);

        }

        internal static void DragonCliffs()
        {
            var scenery = GameObject.Find("island 9 E (dragon cliffs) scenery");
            if (scenery == null)
            {
                LogError("Dragon Cliffs scenery not found.");
                return;
            }

            MakeShopItem("shop item spawner (300)", scenery.transform, new Vector3(-81.859f, 4.379f, -549.696f), new Vector3(358f, 308f, 260f), AssetLoader.Hammer);
            MakeShopItem("shop item spawner (301)", scenery.transform, new Vector3(-81.333f, 3.715f, -549.545f), new Vector3(0f, 224f, 0f), AssetLoader.SealingNails);
            MakeShopItem("shop item spawner (302)", scenery.transform, new Vector3(-80.973f, 3.715f, -549.895f), new Vector3(0f, 224f, 0f), AssetLoader.SealingNails);
            var itemToMove = scenery.GetComponentsInChildren<Transform>()?.FirstOrDefault(t => t.name == "shop item spawner (158)");
            if (itemToMove != null)
                itemToMove.localPosition = new Vector3(-85f, 4.116f, -548.19f);
            MakeShopItem("shop item spawner (303)", scenery.transform, new Vector3(-86.233f, 4.116f, -547.05f), new Vector3(90f, 44.117f, 270f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item spawner (304)", scenery.transform, new Vector3(-81.403f, 4.216f, -551.395f), new Vector3(0f, 134.117f, 270f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item spawner (305)", scenery.transform, new Vector3(-82.403f, 4.216f, -550.395f), new Vector3(0f, 134.117f, 270f), AssetLoader.EmptyCrate);

        }

        internal static void FireFishTown()
        {
            var scenery = GameObject.Find("island lagoon Temple scenery");
            if (scenery == null)
            {
                LogError("Fire Fish Town scenery not found.");
                return;
            }

            MakeShopItem("shop item (300)", scenery.transform, new Vector3(-4.05f, 0.515f, -2.8f), new Vector3(358f, 0f, 90f), AssetLoader.Hammer);
            MakeShopItem("shop item (301)", scenery.transform, new Vector3(-4.1f, 0.365f, -2.97f), new Vector3(0f, 276f, 0f), AssetLoader.SealingNails);
            MakeShopItem("shop item (302)", scenery.transform, new Vector3(-4.05f, 0.365f, -2.485f), new Vector3(0f, 276f, 0f), AssetLoader.SealingNails);
            MakeShopItem("shop item (303)", scenery.transform, new Vector3(-4.4767f, 0.24f, 0.15f), new Vector3(0f, 5.3656f, 0f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item (304)", scenery.transform, new Vector3(-7f, 0.2f, 0.75f), new Vector3(0f, 95.3656f, 0f), AssetLoader.EmptyCrate);
            MakeShopItem("shop item (305)", scenery.transform, new Vector3(-7f, 0.88f, 0.75f), new Vector3(0f, 95.3656f, 0f), AssetLoader.EmptyCrate);
        }

        private static void MakeShopItem(string name, Transform parent, Vector3 position, Vector3 rotation, GameObject go)
        {
            var shopitem = new GameObject(name);
            shopitem.transform.parent = parent;
            shopitem.transform.localPosition = position;
            shopitem.transform.localRotation = Quaternion.Euler(rotation);
            var filter = shopitem.AddComponent<MeshFilter>();
            filter.mesh = go.GetComponent<MeshFilter>().mesh;
            shopitem.AddComponent<MeshRenderer>();
            var itemSpawner = shopitem.AddComponent<ShopItemSpawner>();
            itemSpawner.itemPrefab = go;
        }
    }
}
