using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;

namespace PonyPlushies;

public class PluginConfig
{
    public Dictionary<string, PonyConfig> PonyConfigs;

    // General
    public ConfigEntry<bool> TwoHandedPonies;
    public ConfigEntry<bool> AllowDineSpawning;

    // Mystery Box
    public ConfigEntry<bool> MysteryBoxSpawnEnabled;
    public ConfigEntry<int> MysteryBoxSpawnWeight;
    public ConfigEntry<bool> MysteryBoxStoreEnabled;
    public ConfigEntry<int> MysteryBoxStorePrice;
    public ConfigEntry<float> MysteryBoxSellbackMultiplier;

    // Wesley's Moons Compat
    public ConfigEntry<float> ToyStorePonyMultiplier;
    public ConfigEntry<float> ToyStoreMysteryBoxMultiplier;

    public PluginConfig(ConfigFile config)
    {
        PonyConfigs = new Dictionary<string, PonyConfig>();

        config.SaveOnConfigSet = false;

        // General
        TwoHandedPonies = config.Bind(
            "General",
            "TwoHandedPonies",
            true,
            "Whether some larger ponies (e.g. Celestia, Luna, Queen Chrysalis) are considered two-handed items"
        );
        LethalConfigManager.AddConfigItem(
            new BoolCheckBoxConfigItem(TwoHandedPonies, new BoolCheckBoxOptions
            {
                Section = "General",
                Name = "Two Handed Ponies",
                Description = "Whether some larger ponies (e.g. Celestia, Luna, Queen Chrysalis) are considered two-handed items",
            })
        );

        AllowDineSpawning = config.Bind(
            "General",
            "AllowDineSpawning",
            true,
            "Whether ponies can spawn on Dine. In vanilla, Dine is supposed to only spawn body parts as scrap. With this enabled, ponies will spawn frequently on Dine due to the limited loot pool on this moon."
        );
        LethalConfigManager.AddConfigItem(
            new BoolCheckBoxConfigItem(AllowDineSpawning, new BoolCheckBoxOptions
            {
                Section = "General",
                Name = "Spawn Ponies on Dine",
                Description = "Whether ponies can spawn on Dine. In vanilla, Dine is supposed to only spawn body parts as scrap. With this enabled, ponies will spawn frequently on Dine due to the limited loot pool on this moon.",
            })
        );

        // Mystery Box
        MysteryBoxSpawnEnabled = config.Bind(
            "MysteryBox",
            "MysteryBoxSpawnEnabled",
            true,
            "Whether Mystery Boxes containing a random pony can spawn as scrap on moons"
        );
        LethalConfigManager.AddConfigItem(
            new BoolCheckBoxConfigItem(MysteryBoxSpawnEnabled, new BoolCheckBoxOptions
            {
                Section = "Mystery Box",
                Name = "Spawn as Scrap",
                Description = "Whether Mystery Boxes containing a random pony can spawn as scrap on moons",
            })
        );

        MysteryBoxSpawnWeight = config.Bind(
            "MysteryBox",
            "MysteryBoxSpawnWeight",
            8,
            "The weighted chance for a Mystery Box to spawn"
        );
        LethalConfigManager.AddConfigItem(
            new IntInputFieldConfigItem(MysteryBoxSpawnWeight, new IntInputFieldOptions
            {
                Section = "Mystery Box",
                Name = "Spawn Weight",
                Description = "The weighted chance for a Mystery Box to spawn",
                Min = 1,
                Max = 1000,
            })
        );

        MysteryBoxStoreEnabled = config.Bind(
            "MysteryBox",
            "MysteryBoxStoreEnabled",
            true,
            "Whether Mystery Boxes can be purchased via the terminal"
        );
        LethalConfigManager.AddConfigItem(
            new BoolCheckBoxConfigItem(MysteryBoxStoreEnabled, new BoolCheckBoxOptions
            {
                Section = "Mystery Box",
                Name = "Purchase from Store",
                Description = "Whether Mystery Boxes can be purchased via the terminal",
            })
        );

        MysteryBoxStorePrice = config.Bind(
            "MysteryBox",
            "MysteryBoxStorePrice",
            100,
            "The price to buy a Mystery Box from the terminal"
        );
        LethalConfigManager.AddConfigItem(
            new IntInputFieldConfigItem(MysteryBoxStorePrice, new IntInputFieldOptions
            {
                Section = "Mystery Box",
                Name = "Store Price",
                Description = "The cost to purchase a Mystery Box via the terminal",
                Min = 1,
                Max = 1000000,
            })
        );

        MysteryBoxSellbackMultiplier = config.Bind(
            "MysteryBox",
            "MysteryBoxSellbackMultiplier",
            0.5f,
            "The scrap value of ponies obtained from purchased Mystery Boxes as a percentage of its original value"
        );
        LethalConfigManager.AddConfigItem(
            new FloatSliderConfigItem(MysteryBoxSellbackMultiplier, new FloatSliderOptions
            {
                Section = "Mystery Box",
                Name = "Purchased Pony Value",
                Description = "The scrap value of ponies obtained from purchased Mystery Boxes as a percentage of the box's original value. A value of 1 means the pony will be the same price you paid to purchase the Mystery Box, and 0 means it will always have no value.",
                Min = 0.0f,
                Max = 1.0f,
            })
        );

        // Wesley's Moons Compat
        ToyStorePonyMultiplier = config.Bind(
            "Compat.WesleysMoons",
            "ToyStorePonyMultiplier",
            3.0f,
            "Multiplies the chance for ponies to spawn in Toy Store interiors by this amount"
        );
        LethalConfigManager.AddConfigItem(
            new FloatSliderConfigItem(ToyStorePonyMultiplier, new FloatSliderOptions
            {
                Section = "Compat - Wesley's Moons",
                Name = "Toy Store Pony Multiplier",
                Description = "Multiplies the chance for ponies to spawn in Toy Store interiors by this amount",
                Min = 1.0f,
                Max = 20.0f,
            })
        );

        ToyStoreMysteryBoxMultiplier = config.Bind(
            "Compat.WesleysMoons",
            "ToyStoreMysteryBoxMultiplier",
            5.0f,
            "Multiplies the chance for Mystery Boxes to spawn in Toy Store interiors by this amount"
        );
        LethalConfigManager.AddConfigItem(
            new FloatSliderConfigItem(ToyStoreMysteryBoxMultiplier, new FloatSliderOptions
            {
                Section = "Compat - Wesley's Moons",
                Name = "Toy Store Mystery Box Multiplier",
                Description = "Multiplies the chance for ponies to spawn in Toy Store interiors by this amount",
                Min = 1.0f,
                Max = 20.0f,
            })
        );

        // Ponies
        InitPonyConfigs(config);

        ClearOrphanedEntries(config);
        config.Save();
        config.SaveOnConfigSet = true;
    }

    private void InitPonyConfigs(ConfigFile config)
    {
        foreach (var pony in PonyTypes.All)
        {
            var item = new PonyConfig
            {
                SpawnEnabled = config.Bind(
                    $"Pony.{pony.Id}",
                    "SpawnAsScrap",
                    pony.Values.SpawnEnabled,
                    "Whether this pony should randomly spawn as scrap"
                ),
                SpawnWeight = config.Bind(
                    $"Pony.{pony.Id}",
                    "SpawnWeight",
                    pony.Values.SpawnWeight,
                    "The weighted chance for this pony to spawn"
                ),
                MinValue = config.Bind(
                    $"Pony.{pony.Id}",
                    "MinValue",
                    pony.Values.MinValue,
                    "The minimum sell value this pony can have"
                ),
                MaxValue = config.Bind(
                    $"Pony.{pony.Id}",
                    "MaxValue",
                    pony.Values.MaxValue,
                    "The maximum sell value this pony can have"
                ),
                StoreEnabled = config.Bind(
                    $"Pony.{pony.Id}",
                    "StoreEnabled",
                    pony.Values.StoreEnabled,
                    "Whether this pony is available to purchase via the terminal"
                ),
                StorePrice = config.Bind(
                    $"Pony.{pony.Id}",
                    "StorePrice",
                    pony.Values.StorePrice,
                    "The cost to purchase this pony via the terminal"
                ),
                MysteryBoxEnabled = config.Bind(
                    $"Pony.{pony.Id}",
                    "MysteryBoxEnabled",
                    pony.Values.MysteryBoxEnabled,
                    "Whether this pony can randomly spawn inside a Mystery Box"
                ),
                MysteryBoxWeight = config.Bind(
                    $"Pony.{pony.Id}",
                    "MysteryBoxWeight",
                    pony.Values.MysteryBoxWeight,
                    "The weighted chance of this pony spawning in a Mystery Box"
                ),
            };

            PonyConfigs.Add(pony.Id, item);

            var section = $"Pony - {pony.ReadableName}";

            LethalConfigManager.AddConfigItem(
                new BoolCheckBoxConfigItem(item.SpawnEnabled, new BoolCheckBoxOptions
                {
                    Section = section,
                    Name = "Spawn as Scrap",
                    Description = "Whether this pony can spawn randomly as scrap on moons",
                })
            );
            LethalConfigManager.AddConfigItem(
                new IntInputFieldConfigItem(item.SpawnWeight, new IntInputFieldOptions
                {
                    Section = section,
                    Name = "Spawn Chance",
                    Description = "The weighted chance for this pony to spawn",
                    Min = 1,
                    Max = 1000,
                })
            );
            LethalConfigManager.AddConfigItem(
                new IntInputFieldConfigItem(item.MinValue, new IntInputFieldOptions
                {
                    Section = section,
                    Name = "Min. Value",
                    Description = "The minimum sell value this pony can have",
                    Min = 0,
                    Max = 1000000,
                })
            );
            LethalConfigManager.AddConfigItem(
                new IntInputFieldConfigItem(item.MaxValue, new IntInputFieldOptions
                {
                    Section = section,
                    Name = "Max. Value",
                    Description = "The maximum sell value this pony can have",
                    Min = 0,
                    Max = 1000000,
                })
            );
            LethalConfigManager.AddConfigItem(
                new BoolCheckBoxConfigItem(item.StoreEnabled, new BoolCheckBoxOptions
                {
                    Section = section,
                    Name = "Purchase from Store",
                    Description = "Whether this pony is available to purchase via the terminal",
                })
            );
            LethalConfigManager.AddConfigItem(
                new IntInputFieldConfigItem(item.StorePrice, new IntInputFieldOptions
                {
                    Section = section,
                    Name = "Store Price",
                    Description = "The cost to purchase this pony via the terminal",
                    Min = 0,
                    Max = 1000000,
                })
            );
            LethalConfigManager.AddConfigItem(
                new BoolCheckBoxConfigItem(item.MysteryBoxEnabled, new BoolCheckBoxOptions
                {
                    Section = section,
                    Name = "Spawn in Mystery Box",
                    Description = "Whether this pony can randomly spawn inside a Mystery Box",
                })
            );
            LethalConfigManager.AddConfigItem(
                new IntInputFieldConfigItem(item.MysteryBoxWeight, new IntInputFieldOptions
                {
                    Section = section,
                    Name = "Mystery Box Chance",
                    Description = "The weighted chance of this pony spawning in a Mystery Box",
                    Min = 1,
                    Max = 1000000,
                })
            );
        }
    }

    static void ClearOrphanedEntries(ConfigFile cfg)
    {
        PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg);
        orphanedEntries.Clear();
    }
}

public record PonyConfig
{
    public ConfigEntry<bool> SpawnEnabled;
    public ConfigEntry<int> SpawnWeight;
    public ConfigEntry<int> MinValue;
    public ConfigEntry<int> MaxValue;
    public ConfigEntry<bool> StoreEnabled;
    public ConfigEntry<int> StorePrice;
    public ConfigEntry<bool> MysteryBoxEnabled;
    public ConfigEntry<int> MysteryBoxWeight;
}

public record PonyConfigValues
{
    public bool SpawnEnabled;
    public int SpawnWeight;
    public int MinValue;
    public int MaxValue;
    public bool StoreEnabled;
    public int StorePrice;
    public bool MysteryBoxEnabled;
    public int MysteryBoxWeight;
}