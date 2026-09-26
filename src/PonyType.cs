namespace PonyPlushies;

public record PonyType
{
    public string Id;
    public string ReadableName;
    public PonyConfigValues Values;
    public Item Item;
}

public class PonyTypes
{
    public static readonly PonyConfigValues DefaultValues = new PonyConfigValues
    {
        SpawnEnabled = true,
        SpawnWeight = 3,
        MinValue = 40,
        MaxValue = 80,
        StoreEnabled = false,
        StorePrice = 80,
        MysteryBoxEnabled = true,
        MysteryBoxWeight = 10,
    };

    public static readonly PonyConfigValues CommonValues = new PonyConfigValues
    {
        SpawnEnabled = DefaultValues.SpawnEnabled,
        SpawnWeight = 6,
        MinValue = DefaultValues.MinValue,
        MaxValue = DefaultValues.MaxValue,
        StoreEnabled = DefaultValues.StoreEnabled,
        StorePrice = DefaultValues.StorePrice,
        MysteryBoxEnabled = DefaultValues.MysteryBoxEnabled,
        MysteryBoxWeight = 30,
    };

    public static readonly PonyConfigValues ExpensiveValues = new PonyConfigValues
    {
        SpawnEnabled = DefaultValues.SpawnEnabled,
        SpawnWeight = DefaultValues.SpawnWeight,
        MinValue = 100,
        MaxValue = 160,
        StoreEnabled = DefaultValues.StoreEnabled,
        StorePrice = DefaultValues.StorePrice,
        MysteryBoxEnabled = DefaultValues.MysteryBoxEnabled,
        MysteryBoxWeight = DefaultValues.MysteryBoxWeight,
    };

    public static readonly PonyType[] All = [
        new PonyType {
            Id = "AppleBloom",
            ReadableName = "Apple Bloom",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Applejack",
            ReadableName = "Applejack",
            Values = CommonValues,
        },
        new PonyType {
            Id = "AutumnBlaze",
            ReadableName = "Autumn Blaze",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "BabsSeed",
            ReadableName = "Babs Seed",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "BerryPunch",
            ReadableName = "Berry Punch",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "BigMac",
            ReadableName = "Big Mac",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "BonBon",
            ReadableName = "Bon Bon",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Braeburn",
            ReadableName = "Braeburn",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Cadance",
            ReadableName = "Princess Cadance",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "Celestia",
            ReadableName = "Princess Celestia",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "CheeseSandwich",
            ReadableName = "Cheese Sandwich",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Chrysalis",
            ReadableName = "Queen Chrysalis",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "Daybreaker",
            ReadableName = "Daybreaker",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "Derpy",
            ReadableName = "Derpy",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Discord",
            ReadableName = "Discord",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "DrHooves",
            ReadableName = "Dr. Hooves",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Fizzy",
            ReadableName = "Fizzy",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Fluttershy",
            ReadableName = "Fluttershy",
            Values = CommonValues,
        },
        new PonyType {
            Id = "GrannySmith",
            ReadableName = "Granny Smith",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Kerfuffle",
            ReadableName = "Kerfuffle",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "KingSombra",
            ReadableName = "King Sombra",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "Littlepip",
            ReadableName = "Littlepip",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Luna",
            ReadableName = "Princess Luna",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "Lyra",
            ReadableName = "Lyra",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "MaudPie",
            ReadableName = "Maud Pie",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Minuette",
            ReadableName = "Minuette",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "MoonDancer",
            ReadableName = "Moon Dancer",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "NightmareMoon",
            ReadableName = "Nightmare Moon",
            Values = ExpensiveValues,
        },
        new PonyType {
            Id = "Octavia",
            ReadableName = "Octavia",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "PinkiePie",
            ReadableName = "Pinkie Pie",
            Values = CommonValues,
        },
        new PonyType {
            Id = "PunkRarity",
            ReadableName = "Punk Rarity",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "RainbowDash",
            ReadableName = "Rainbow Dash",
            Values = CommonValues,
        },
        new PonyType {
            Id = "Rarity",
            ReadableName = "Rarity",
            Values = CommonValues,
        },
        new PonyType {
            Id = "Scootaloo",
            ReadableName = "Scootaloo",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "ShiningArmor",
            ReadableName = "Shining Armor",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Snowdrop",
            ReadableName = "Snowdrop",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Spike",
            ReadableName = "Spike",
            Values = new PonyConfigValues
            {
                SpawnEnabled = true,
                SpawnWeight = DefaultValues.SpawnWeight,
                MinValue = 20,
                MaxValue = 50,
                StoreEnabled = DefaultValues.StoreEnabled,
                StorePrice = DefaultValues.StorePrice,
                MysteryBoxEnabled = DefaultValues.MysteryBoxEnabled,
                MysteryBoxWeight = DefaultValues.MysteryBoxWeight,
            },
        },
        new PonyType {
            Id = "StarlightGlimmer",
            ReadableName = "Starlight Glimmer",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "StoneyPony",
            ReadableName = "Stoney Pony",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Sunburst",
            ReadableName = "Sunburst",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "SunsetShimmer",
            ReadableName = "Sunset Shimmer",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "SweetieBelle",
            ReadableName = "Sweetie Belle",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "TempestShadow",
            ReadableName = "Tempest Shadow",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Trixie",
            ReadableName = "Trixie",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "TwilightSparkle",
            ReadableName = "Twilight Sparkle",
            Values = CommonValues,
        },
        new PonyType {
            Id = "VinylScratch",
            ReadableName = "Vinyl Scratch",
            Values = DefaultValues,
        },
        new PonyType {
            Id = "Zecora",
            ReadableName = "Zecora",
            Values = DefaultValues,
        },
    ];
}