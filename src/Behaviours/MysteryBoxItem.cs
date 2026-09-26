using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace PonyPlushies;

public class MysteryBoxItem : GiftBoxItem
{

    public bool IsShopItem = false;

    public override void InitializeAfterPositioning()
    {
        base.InitializeAfterPositioning();
        bool loadedItemFromSave = (bool)AccessTools.Field(typeof(GiftBoxItem), "loadedItemFromSave").GetValue(this);

        if (loadedItemFromSave || !IsServer)
        {
            return;
        }

        System.Random random = new System.Random();

        PonyType[] ponies = PonyTypes.All
            .Where(pony => pony.Item != null)
            .Where(pony => pony.Values.MysteryBoxEnabled == true && pony.Values.MysteryBoxWeight > 0)
            .ToArray();

        if (ponies.Length <= 0)
        {
            Plugin.logger.LogWarning("Mystery Box has 0 ponies to choose from, nothing will be in it");

            AccessTools.Field(typeof(GiftBoxItem), "objectInPresentItem").SetValue(this, null);
            AccessTools.Field(typeof(GiftBoxItem), "objectInPresent").SetValue(this, null);
            AccessTools.Field(typeof(GiftBoxItem), "objectInPresentValue").SetValue(this, 0);

            return;
        }

        List<int> weights = new List<int>();
        foreach (PonyType pony in ponies)
        {
            weights.Add(pony.Values.MysteryBoxWeight);
        }

        int randomWeightedIndexList = RoundManager.Instance.GetRandomWeightedIndexList(weights, random);

        Item objectInPresentItem = ponies[randomWeightedIndexList].Item;
        GameObject objectInPresent = objectInPresentItem.spawnPrefab;
        int objectInPresentValue = 0;

        if (IsShopItem)
        {
            Terminal terminal = FindObjectOfType<Terminal>();

            int itemIndex = Array.IndexOf(terminal.buyableItemsList, PluginItems.MysteryBoxStoreItem);
            float saleMultiplier = terminal.itemSalesPercentages[itemIndex] / 100.0f;

            int basePrice = Plugin.BoundConfig.MysteryBoxStorePrice.Value;
            float sellbackMultiplier = Plugin.BoundConfig.MysteryBoxSellbackMultiplier.Value;

            objectInPresentValue = (int)(basePrice * sellbackMultiplier * saleMultiplier);
        }
        else
        {
            objectInPresentValue = (int)(random.Next(objectInPresentItem.minValue, objectInPresentItem.maxValue) * RoundManager.Instance.scrapValueMultiplier);
        }

        AccessTools.Field(typeof(GiftBoxItem), "objectInPresentItem").SetValue(this, objectInPresentItem);
        AccessTools.Field(typeof(GiftBoxItem), "objectInPresent").SetValue(this, objectInPresent);
        AccessTools.Field(typeof(GiftBoxItem), "objectInPresentValue").SetValue(this, objectInPresentValue);
    }

}