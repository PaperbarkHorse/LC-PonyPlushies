using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace PonyPlushies;

public class MysteryBoxItem : GiftBoxItem
{

    public bool IsShopItem = false;

    private bool opened = false;
    private float despawnTimer = 0;

    public override void InitializeAfterPositioning()
    {
        bool loadedItemFromSave = (bool)AccessTools.Field(typeof(GiftBoxItem), "loadedItemFromSave").GetValue(this);

        if (loadedItemFromSave || !IsServer)
        {
            return;
        }

        System.Random random = new System.Random();

        PonyType[] ponies = PonyTypes.All
            .Where(pony => pony.Item != null)
            .Where(pony => Plugin.BoundConfig.PonyConfigs[pony.Id].MysteryBoxEnabled.Value == true)
            .Where(pony => Plugin.BoundConfig.PonyConfigs[pony.Id].MysteryBoxWeight.Value > 0)
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
            weights.Add(Plugin.BoundConfig.PonyConfigs[pony.Id].MysteryBoxWeight.Value);
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

    public override void ItemActivate(bool used, bool buttonDown = true)
    {
        if (IsServer)
        {
            base.ItemActivate(used, buttonDown);
            opened = true;
        }
    }

    public override void Update()
    {
        base.Update();

        if (IsServer)
        {
            if (opened)
            {
                despawnTimer += Time.deltaTime;
            }

            if (despawnTimer > 1)
            {
                DiscardItem();
                NetworkObject.Despawn();
                opened = false;
            }
        }
    }

}