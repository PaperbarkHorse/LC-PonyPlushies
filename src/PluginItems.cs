using System;
using Dawn;
using UnityEngine;

namespace PonyPlushies;

public class PluginItems
{

    public static Item MysteryBoxItem;
    public static Item MysteryBoxStoreItem;

    public static void Init()
    {
        foreach (var pony in PonyTypes.All)
        {
            InitPony(pony);
        }

        MysteryBoxItem = Plugin.ModAssets.LoadAsset<Item>($"{Plugin.AssetLocation}/Items/MysteryBox/MysteryBoxItem.asset");
        GameObject mysteryBoxPrefab = Plugin.ModAssets.LoadAsset<GameObject>($"{Plugin.AssetLocation}/Items/MysteryBox/MysteryBoxPrefab.prefab");

        MysteryBoxItem.minValue = (int)(PonyTypes.DefaultValues.MinValue / 0.4f);
        MysteryBoxItem.maxValue = (int)(PonyTypes.DefaultValues.MaxValue / 0.4f);

        if (mysteryBoxPrefab == null)
        {
            Plugin.logger.LogError("Failed to load Mystery Box - prefab not found in asset bundle");
        }

        Plugin.PrefabsToRegsiter.Add(mysteryBoxPrefab);

        DawnLib.DefineItem(
            NamespacedKey<DawnItemInfo>.From("ponyplushies", "MysteryBox"),
            MysteryBoxItem,
            builder =>
            {
                if (Plugin.BoundConfig.MysteryBoxSpawnEnabled.Value == true)
                {
                    builder.DefineScrap(scrap => scrap
                        .SetWeights(weights =>
                        {
                            weights.SetGlobalWeight(new MysteryBoxSpawnWeightProvider());
                        })
                    );
                }
            }
        );

        MysteryBoxStoreItem = Plugin.ModAssets.LoadAsset<Item>($"{Plugin.AssetLocation}/Items/MysteryBox/MysteryBoxStoreItem.asset");
        GameObject mysteryBoxStorePrefab = Plugin.ModAssets.LoadAsset<GameObject>($"{Plugin.AssetLocation}/Items/MysteryBox/MysteryBoxStorePrefab.prefab");

        if (mysteryBoxStorePrefab == null)
        {
            Plugin.logger.LogError("Failed to load Mystery Box - prefab not found in asset bundle");
        }

        Plugin.PrefabsToRegsiter.Add(mysteryBoxStorePrefab);


        DawnLib.DefineItem(
            NamespacedKey<DawnItemInfo>.From("ponyplushies", "MysteryBoxStore"),
            MysteryBoxStoreItem,
            builder =>
            {
                if (Plugin.BoundConfig.MysteryBoxStoreEnabled.Value == true)
                {
                    builder.DefineShop(shop => shop
                        .OverrideCost(Plugin.BoundConfig.MysteryBoxStorePrice.Value)
                    );
                }
            }
        );
    }

    private static void InitPony(PonyType pony)
    {
        Plugin.logger.LogDebug($"Initialising pony {pony.Id}");

        Item item = Plugin.ModAssets.LoadAsset<Item>($"{Plugin.AssetLocation}/Ponies/{pony.Id}/{pony.Id}Item.asset");
        GameObject prefab = Plugin.ModAssets.LoadAsset<GameObject>($"{Plugin.AssetLocation}/Ponies/{pony.Id}/{pony.Id}Prefab.prefab");
        PonyConfig config = Plugin.BoundConfig.PonyConfigs[pony.Id];

        if (item == null)
        {
            Plugin.logger.LogError($"Failed to load pony {pony.Id} - asset not found in bundle");
            return;
        }

        if (prefab == null)
        {
            Plugin.logger.LogError($"Failed to load pony {pony.Id} - prefab not found in bundle");
            return;
        }

        if (config == null)
        {
            Plugin.logger.LogError($"Failed to load pony {pony.Id} - config values not found");
            return;
        }

        Plugin.PrefabsToRegsiter.Add(prefab);

        item.minValue = (int)Math.Round(config.MinValue.Value / 0.4);
        item.maxValue = (int)Math.Round(config.MaxValue.Value / 0.4);

        if (!Plugin.BoundConfig.TwoHandedPonies.Value && item.twoHanded)
        {
            item.twoHanded = false;
        }

        pony.Item = item;

        DawnLib.DefineItem(
            NamespacedKey<DawnItemInfo>.From("ponyplushies", pony.Id),
            item,
            builder =>
            {
                if (config.SpawnEnabled.Value == true)
                {
                    builder.DefineScrap(scrap => scrap
                        .SetWeights(weights =>
                        {
                            weights.SetGlobalWeight(new PonySpawnWeightProvider(pony));
                        })
                    );
                }

                if (config.StoreEnabled.Value == true)
                {
                    builder.DefineShop(shop => shop
                        .OverrideCost(config.StorePrice.Value)
                    );
                }
            }
        );
    }

}

public class PonySpawnWeightProvider : IContextualProvider<int?, DawnMoonInfo, SpawnWeightContext>
{
    private PonyType pony;

    public PonySpawnWeightProvider(PonyType pony)
    {
        this.pony = pony;
    }

    public int? Provide(DawnMoonInfo info, SpawnWeightContext ctx)
    {
        int weight = Plugin.BoundConfig.PonyConfigs[pony.Id].SpawnWeight.Value;

        if (Plugin.BoundConfig.AllowDineSpawning.Value == false && ctx.Moon.Key.ToString() == "lethal_company:dine")
        {
            return 0;
        }

        if (ctx.Dungeon.Key.ToString() == "toy_store:toystoreflow")
        {
            weight = (int)(weight * Plugin.BoundConfig.ToyStorePonyMultiplier.Value);
        }

        return weight;
    }
}

public class MysteryBoxSpawnWeightProvider : IContextualProvider<int?, DawnMoonInfo, SpawnWeightContext>
{
    public int? Provide(DawnMoonInfo info, SpawnWeightContext ctx)
    {
        int weight = Plugin.BoundConfig.MysteryBoxSpawnWeight.Value;

        if (Plugin.BoundConfig.AllowDineSpawning.Value == false && ctx.Moon.Key.ToString() == "lethal_company:dine")
        {
            return 0;
        }

        if (ctx.Dungeon.Key.ToString() == "toy_store:toystoreflow")
        {
            weight = (int)(weight * Plugin.BoundConfig.ToyStoreMysteryBoxMultiplier.Value);
        }

        return weight;
    }
}