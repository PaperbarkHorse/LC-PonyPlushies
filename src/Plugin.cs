using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Dawn;
using HarmonyLib;
using UnityEngine;

namespace PonyPlushies;

[BepInPlugin(modGUID, modName, modVersion)]
[BepInDependency("ainavt.lc.lethalconfig")]
public class Plugin : BaseUnityPlugin
{
    public const string modGUID = "horse.paperbark.PonyPlushies";
    public const string modName = "PonyPlushies";
    public const string modVersion = "0.0.0";

    private static Harmony harmony = new Harmony(modGUID);
    internal static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(modName);
    internal static PluginConfig BoundConfig { get; private set; } = null!;

    public static readonly NamespacedKey<DawnItemInfo> TabletItem = NamespacedKey<DawnItemInfo>.From("ponyplushies", "tablet");

    public static AssetBundle ModAssets;
    public static readonly string AssetLocation = "assets/LethalCompany/Mods/plugins/PonyPlushies";

    void Awake()
    {
        BoundConfig = new PluginConfig(Config);

        PrepareNetcode();
        LoadModAssets();
        ApplyPatches();

        InitItems();

        logger.LogInfo("Pony Plushies " + modVersion + " is loaded! /)");
    }

    private void LoadModAssets()
    {
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        ModAssets = AssetBundle.LoadFromFile(Path.Combine(assemblyPath, "creaturecounters"));

        if (ModAssets == null)
        {
            logger.LogError("Failed to load mod asset bundle");
        }
    }

    private void ApplyPatches()
    {
        harmony.PatchAll();
    }

    private void PrepareNetcode()
    {
        // Source: https://lethal.wiki/dev/advanced/networking/objects
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                if (attributes.Length > 0)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }

    private void InitItems()
    {
        DawnLib.DefineItem(TabletItem, ModAssets.LoadAsset<Item>(AssetLocation + "/Items/Tablet/TabletItem.asset"), builder => builder
            .DefineScrap(scrap => scrap
                .SetWeights(weights => weights
                    .SetGlobalWeight(5)
                )
            )
            .DefineShop(shop => shop
                .OverrideCost(150)
            )
        );
    }
}
