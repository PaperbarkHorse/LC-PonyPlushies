using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;

namespace PonyPlushies;

class PluginConfig
{
    // public readonly ConfigEntry<bool> vainInfestationEnabled;

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;

        // vainInfestationEnabled = config.Bind(
        //     "Tweaks.VainInfestation",
        //     "Enabled",
        //     true,
        //     "Whether random Vain Shroud infestations should replace the vanilla spreading mechanics"
        // );

        ClearOrphanedEntries(config);
        config.Save();
        config.SaveOnConfigSet = true;

        // LethalConfigManager.AddConfigItem(
        //     new BoolCheckBoxConfigItem(vainInfestationEnabled, new BoolCheckBoxOptions
        //     {
        //         Section = "Vain Shroud Infestations",
        //         Name = "Enabled",
        //         Description = "When enabled, Vain Shrouds have a random chance of occuring each round instead of vanilla's spawning where they stay between days."
        //     })
        // );
    }

    static void ClearOrphanedEntries(ConfigFile cfg)
    {
        PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg);
        orphanedEntries.Clear();
    }
}
