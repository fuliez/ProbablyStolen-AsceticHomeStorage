using System;
using HarmonyLib;

namespace ProbablyStolen.AsceticHomeStorage
{
    internal static class StoragePolicy
    {
        private static bool reportedFailure;
        internal static void ResetDiagnostics() => reportedFailure = false;

        internal static bool Eligible(PlayerStore? store) =>
            ModEntry.Enabled && store != null && store.IsPropertyPaid && StartingPerk.IsAscetic();

        internal static void Restore(EmporiumEntry entry)
        {
            // Vanilla initialization is 24x10. SetBigInv is 28x10 and is NOT the baseline.
            // Only enlarge the exact ascetic grid; never shrink or overwrite other layouts.
            var grid = entry.invElement;
            if (grid == null || entry.invWindow == null || grid.inventoryShape == null) return;
            if (grid.inventoryShape.width != 16 || grid.inventoryShape.height != 10) return;
            grid.SetShape(24, 10);
            grid.Validate();
            entry.invWindow.Validate();
        }

        internal static void Warn(Exception exception)
        {
            if (reportedFailure) return;
            reportedFailure = true;
            ModEntry.Log?.Warning("Cannot restore ascetic storage: " + exception);
        }
    }

    [HarmonyPatch(typeof(EmporiumEntry), nameof(EmporiumEntry.SetSmallInv))]
    internal static class SmallInventoryPatch
    {
        [HarmonyPrefix]
        internal static bool Prefix(EmporiumEntry __instance)
        {
            try
            {
                if (!StoragePolicy.Eligible(PlayerStore.Instance)) return true;
                // Loading calls this BEFORE decoding inventory items. Avoid even a transient shrink.
                StoragePolicy.Restore(__instance);
                return false;
            }
            catch (Exception exception)
            {
                StoragePolicy.Warn(exception);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(PlayerStore), nameof(PlayerStore.OnStoreOwned))]
    internal static class StoreOwnedPatch
    {
        [HarmonyPostfix]
        internal static void Postfix(PlayerStore __instance)
        {
            try
            {
                if (StoragePolicy.Eligible(__instance) && EmporiumEntry.Instance != null)
                    StoragePolicy.Restore(EmporiumEntry.Instance);
            }
            catch (Exception exception) { StoragePolicy.Warn(exception); }
        }
    }
}
