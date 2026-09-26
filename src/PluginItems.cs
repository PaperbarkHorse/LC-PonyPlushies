using System;
using Dawn;

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
        MysteryBoxItem.minValue = (int)(PonyTypes.DefaultValues.MinValue / 0.4f);
        MysteryBoxItem.maxValue = (int)(PonyTypes.DefaultValues.MaxValue / 0.4f);

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
                            weights.SetGlobalWeight(Plugin.BoundConfig.MysteryBoxSpawnWeight.Value);
                        })
                    );
                }
            }
        );

        MysteryBoxStoreItem = Plugin.ModAssets.LoadAsset<Item>($"{Plugin.AssetLocation}/Items/MysteryBox/MysteryBoxStoreItem.asset");
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
        PonyConfig config = Plugin.BoundConfig.PonyConfigs[pony.Id];

        if (item == null)
        {
            Plugin.logger.LogError($"Failed to load pony {pony.Id} - asset not found in bundle");
            return;
        }

        if (config == null)
        {
            Plugin.logger.LogError($"Failed to load pony {pony.Id} - config values not found");
            return;
        }

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
                            weights.SetGlobalWeight(config.SpawnWeight.Value);
                            // TODO: Tags + integrations
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