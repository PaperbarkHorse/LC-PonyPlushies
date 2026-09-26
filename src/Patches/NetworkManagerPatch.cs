using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace PonyPlushies;

[HarmonyPatch]
public class NetworkManagerPatch
{
    [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), "Start")]
    public static void Start_PostfixPatch()
    {
        foreach (GameObject prefab in Plugin.PrefabsToRegsiter)
        {
            if (prefab == null)
            {
                Plugin.logger.LogError("Attempted to register null prefab");
                continue;
            }

            NetworkManager.Singleton.AddNetworkPrefab(prefab);
        }
    }
}