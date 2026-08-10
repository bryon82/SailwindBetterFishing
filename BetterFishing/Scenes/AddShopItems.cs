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
            if (scene.name == "island 20 A (Oasis)")
                Oasis();
            if (scene.name == "island 18 M (Oasis)")
                HappyBay();
        }

        internal static void GoldRockCity()
        {
            var scenery = GameObject.Find("island 1 A (gold rock) scenery");
            if (scenery == null)
            {
                LogError("Gold Rock City scenery not found.");
                return;
            }

            var shopkeeper = scenery.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "rad shopkeeper");
            if (shopkeeper == null)
            {
                // resize shop (10) local scale to fit new stall
                var shop10 = scenery.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "shop (10)");
                if (shop10 != null)
                {
                    shop10.localScale = new Vector3(21.78927f, 13.92925f, 12.39353f);
                    shop10.localPosition = new Vector3(1558.77f, 8.4f, -361.33f);
                }

                var shopPos = new Vector3(1545f, 7.21f, -361.5f);
                var shopRot = new Vector3(270f, 238f, 0f);
                var shopkeeperPos = new Vector3(1544f, 5.06f, -360f);
                var shopkeeperRot = new Vector3(0f, 140f, 0f);
                AddShopStall(scenery, "market_stall (10)", "shop (11)", shopPos, shopRot, "shopkeeper (11)", shopkeeperPos, shopkeeperRot);
            }

            MakeShopItem("shop item 301", scenery.transform, new Vector3(1543.85f, 7f, -363.4f), new Vector3(349f, 328f, 0f), Items.SealingNails);
            MakeShopItem("shop item 302", scenery.transform, new Vector3(1543.4f, 7.17f, -362.7f), new Vector3(349f, 328f, 0f), Items.SealingNails);
            MakeShopItem("shop item 303", scenery.transform, new Vector3(1545.3f, 5f, -362.08f), new Vector3(0f, 328f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item 304", scenery.transform, new Vector3(1541.6f, 5.55f, -364.3f), new Vector3(0f, 238f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item 305", scenery.transform, new Vector3(1541.6f, 6.935f, -364.3f), new Vector3(0f, 238f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item 306", scenery.transform, new Vector3(1545.55f, 6.86f, -362.16f), new Vector3(80f, 0f, 0f), Items.SpoonLure);
            MakeShopItem("shop item 307", scenery.transform, new Vector3(1545.25f, 7f, -361.66f), new Vector3(80f, 0f, 0f), Items.SpoonLure);
            MakeShopItem("shop item 308", scenery.transform, new Vector3(1544.8f, 7f, -362.8f), new Vector3(349f, 328f, 0f), Items.CrateSpoonLures);
            MakeShopItem("shop item 309", scenery.transform, new Vector3(1544.35f, 5.87f, -362.1f), new Vector3(349f, 328f, 0f), Items.CrateSpoonLures);
        }

        internal static void FortAestrin()
        {
            var scenery = GameObject.Find("island 15 M (Fort) scenery");
            if (scenery == null)
            {
                LogError("Fort Aestrin scenery not found.");
                return;
            }

            var shopkeeper = scenery.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "rad shopkeeper");
            if (shopkeeper == null)
            {
                var shopPos = new Vector3(-47.74f, 2.26f, 44.77f);
                var shopRot = new Vector3(270f, 359.7961f, 0f);
                var shopkeeperPos = new Vector3(-47.74f, 2.1f, 43.5f);
                var shopkeeperRot = new Vector3(0f, 359.7961f, 0f);
                AddShopStall(scenery, "market stall medi 2 (2)", "shop area (13)", shopPos, shopRot, "shopkeeper (3)", shopkeeperPos, shopkeeperRot);
                var shop = scenery.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "rad shop");
                shop.localScale = new Vector3(6f, 6f, 6f);

                // ft. aestrin shops have banners
                var bannerPost = scenery.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "banner M post");
                if (bannerPost != null)
                {
                    var bp = GameObject.Instantiate(bannerPost.gameObject, scenery.transform);
                    bp.name = "rad banner";
                    bp.transform.localPosition = new Vector3(-44.5009f, 2.43f, 46.2771f);
                    bp.GetComponent<MeshRenderer>().enabled = true;
                    bp.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                }
            }

            MakeShopItem("shop item (301)", scenery.transform, new Vector3(-46.8f, 2.97f, 44.9f), new Vector3(350f, 180f, 0f), Items.SealingNails);
            MakeShopItem("shop item (302)", scenery.transform, new Vector3(-46.8f, 3.04f, 44.5f), new Vector3(350f, 180f, 0f), Items.SealingNails);
            MakeShopItem("shop item (303)", scenery.transform, new Vector3(-45.95f, 2.16f, 44.5f), new Vector3(0f, 90f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (304)", scenery.transform, new Vector3(-45.95f, 2.72f, 44.5f), new Vector3(0f, 90f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (305)", scenery.transform, new Vector3(-49.6f, 2.1f, 44.5f), new Vector3(0f, 90f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (306)", scenery.transform, new Vector3(-47.77f, 2.85f, 44.95f), new Vector3(80f, 214f, 0f), Items.TopwaterLure);
            MakeShopItem("shop item (307)", scenery.transform, new Vector3(-47.77f, 2.89f, 44.7f), new Vector3(80f, 214f, 0f), Items.TopwaterLure);
            MakeShopItem("shop item (308)", scenery.transform, new Vector3(-47.35f, 2.97f, 44.9f), new Vector3(350f, 180f, 0f), Items.CrateTopwaterLures);
            MakeShopItem("shop item (309)", scenery.transform, new Vector3(-47.35f, 3.04f, 44.5f), new Vector3(350f, 180f, 0f), Items.CrateTopwaterLures);
        }

        internal static void DragonCliffs()
        {
            var scenery = GameObject.Find("island 9 E (dragon cliffs) scenery");
            if (scenery == null)
            {
                LogError("Dragon Cliffs scenery not found.");
                return;
            }

            var shopkeeper = scenery.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "rad shopkeeper");
            if (shopkeeper == null)
            {
                var shopPos = new Vector3(-73.134f, 4.68f, -552.089f);
                var shopRot = new Vector3(270f, 45f, 0f);
                var shopkeeperPos = new Vector3(-72.574f, 3.603f, -552.519f);
                var shopkeeperRot = new Vector3(0f, 313.5019f, 0f);
                AddShopStall(scenery, "market_stall", "shop area (8)", shopPos, shopRot, "shopkeeper (3)", shopkeeperPos, shopkeeperRot);
            }

            MakeShopItem("shop item spawner (301)", scenery.transform, new Vector3(-72.85f, 4.59f, -551.15f), new Vector3(345f, 135f, 0f), Items.SealingNails);
            MakeShopItem("shop item spawner (302)", scenery.transform, new Vector3(-72.55f, 4.685f, -551.45f), new Vector3(345f, 135f, 0f), Items.SealingNails);            
            MakeShopItem("shop item spawner (303)", scenery.transform, new Vector3(-71.9f, 3.65f, -550.8f), new Vector3(0f, 45f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item spawner (304)", scenery.transform, new Vector3(-71.15f, 3.65f, -550.05f), new Vector3(0f, 45f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item spawner (305)", scenery.transform, new Vector3(-71.5f, 4.35f, -550.4f), new Vector3(0f, 45f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item spawner (306)", scenery.transform, new Vector3(-73.1f, 4.655f, -552.25f), new Vector3(90f, 214.8614f, 0f), Items.SwimbaitLure);
            MakeShopItem("shop item spawner (307)", scenery.transform, new Vector3(-73.2f, 4.649f, -552.15f), new Vector3(90f, 214.8614f, 0f), Items.SwimbaitLure);
            MakeShopItem("shop item spawner (308)", scenery.transform, new Vector3(-73.2f, 4.59f, -551.5f), new Vector3(345f, 135f, 0f), Items.CrateSwimbaitLures);
            MakeShopItem("shop item spawner (309)", scenery.transform, new Vector3(-72.9f, 4.685f, -551.8f), new Vector3(345f, 135f, 0f), Items.CrateSwimbaitLures);
        }

        internal static void FireFishTown()
        {
            var scenery = GameObject.Find("island lagoon Temple scenery");
            if (scenery == null)
            {
                LogError("Fire Fish Town scenery not found.");
                return;
            }

            MakeShopItem("shop item (301)", scenery.transform, new Vector3(-4.1f, 0.365f, -2.97f), new Vector3(0f, 276f, 0f), Items.SealingNails);
            MakeShopItem("shop item (302)", scenery.transform, new Vector3(-4.05f, 0.365f, -2.485f), new Vector3(0f, 276f, 0f), Items.SealingNails);
            MakeShopItem("shop item (303)", scenery.transform, new Vector3(-4.4767f, 0.24f, 0.15f), new Vector3(0f, 5.3656f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (304)", scenery.transform, new Vector3(-7f, 0.2f, 0.75f), new Vector3(0f, 95.3656f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (305)", scenery.transform, new Vector3(-7f, 0.88f, 0.75f), new Vector3(0f, 95.3656f, 0f), Items.EmptyCrate);
        }

        internal static void Oasis()
        {
            var scenery = GameObject.Find("island 20 A (oasis) scenery");
            if (scenery == null)
            {
                LogError("Oasis scenery not found.");
                return;
            }

            MakeShopItem("shop item (301)", scenery.transform, new Vector3(78.174f, 15.798f, -142.55f), new Vector3(270f, 180f, 0f), Items.SealingNails);
            MakeShopItem("shop item (302)", scenery.transform, new Vector3(78.073f, 15.45f, -144f), new Vector3(0f, 89.1775f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (303)", scenery.transform, new Vector3(78.073f, 15.45f, -146.7f), new Vector3(0f, 89.1775f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (304)", scenery.transform, new Vector3(78.073f, 15.45f, -149.4f), new Vector3(0f, 89.1775f, 0f), Items.EmptyCrate);
        }

        internal static void HappyBay()
        {
            var scenery = GameObject.Find("island 18 M (Oasis) scenery");
            if (scenery == null)
            {
                LogError("Happy Bay scenery not found.");
                return;
            }

            MakeShopItem("shop item (301)", scenery.transform, new Vector3(-137.68f, 5.58f, 22.87f), new Vector3(270f, 210f, 0f), Items.SealingNails);
            MakeShopItem("shop item (302)", scenery.transform, new Vector3(-138.268f, 4.71f, 21.9f), new Vector3(0f, 30f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (303)", scenery.transform, new Vector3(-137.74f, 4.71f, 22.84f), new Vector3(0f, 30f, 0f), Items.EmptyCrate);
            MakeShopItem("shop item (304)", scenery.transform, new Vector3(-138.04f, 5.41f, 22.34f), new Vector3(0f, 30f, 0f), Items.EmptyCrate);
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

        private static void AddShopStall(GameObject scenery, string templateStallName, string templateShop, Vector3 pos, Vector3 rot, string templateShopkeeper, Vector3 shopkeeperPos, Vector3 shopkeeperRot)
        {
            var stallTemplate = scenery.GetComponentsInChildren<Transform>()?.FirstOrDefault(t => t.name == templateStallName);
            var stall = GameObject.Instantiate(stallTemplate.gameObject, scenery.transform);
            stall.name = "rad market stall";
            stall.transform.localPosition = pos;
            stall.transform.localRotation = Quaternion.Euler(rot);
            stall.GetComponent<MeshRenderer>().enabled = true;

            var shopTemplate = scenery.GetComponentsInChildren<Transform>()?.FirstOrDefault(t => t.name == templateShop);
            var shop = GameObject.Instantiate(shopTemplate.gameObject, scenery.transform);
            shop.transform.localPosition = pos;
            shop.transform.localRotation = Quaternion.Euler(rot);
            shop.name = "rad shop";
            var shopArea = shop.GetComponent<ShopArea>();
            shopArea.itemsForSale.Clear();

            var shopkeeperTemplate = scenery.GetComponentsInChildren<Transform>()?.FirstOrDefault(t => t.name == templateShopkeeper);
            var shopkeeper = GameObject.Instantiate(shopkeeperTemplate.gameObject, scenery.transform);
            shopkeeper.transform.localPosition = shopkeeperPos;
            shopkeeper.transform.localRotation = Quaternion.Euler(shopkeeperRot);
            shopkeeper.name = "rad shopkeeper";
            shopkeeper.SetPrivateField("shopLocalPos", pos);
            shopkeeper.SetPrivateField("shopRotation", Quaternion.Euler(rot));
            shopArea.SetPrivateField("shopkeeper", shopkeeper.GetComponent<Shopkeeper>());
            shopkeeper.SetPrivateField("shop", shopArea);
        }
    }
}
