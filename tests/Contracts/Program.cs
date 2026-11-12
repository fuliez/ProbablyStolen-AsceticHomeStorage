// Hand-written doubles test production policy; no game implementation is redistributed.
using System;
using System.Collections.Generic;
using ProbablyStolen.AsceticHomeStorage;

namespace HarmonyLib
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HarmonyPatch : Attribute { public HarmonyPatch(Type type, string method) { } }
    [AttributeUsage(AttributeTargets.Method)] public sealed class HarmonyPrefix : Attribute { }
    [AttributeUsage(AttributeTargets.Method)] public sealed class HarmonyPostfix : Attribute { }
}
public interface IMod { void Init(ModManifest manifest); void OnEnable(); void OnDisable(); }
public sealed class ModManifest { }
public sealed class ModLog
{
    public static int Warnings;
    public ModLog(ModManifest manifest) { }
    public void Log(object text) { }
    public void Warning(object text) => Warnings++;
}
public sealed class PlayerStore
{
    public static PlayerStore? Instance;
    public bool IsPropertyPaid;
    public readonly List<string> startingPerks = new List<string>();
    public int Cash = 123;
    public void OnStoreOwned() { }
}
public static class StartingPerk
{
    public static bool ThrowOnRead;
    public static bool IsAscetic() => ThrowOnRead ? throw new InvalidOperationException("fixture") : PlayerStore.Instance!.startingPerks.Contains("ascetic");
}
public sealed class GridShape
{
    public int width;
    public int height;
}
public sealed class GameGridInventory
{
    public GridShape inventoryShape = new GridShape();
    public readonly List<(int X, int Y)> Items = new List<(int, int)>();
    public int Changes;
    public GameGridInventory(int w, int h) { inventoryShape.width = w; inventoryShape.height = h; }
    public void SetShape(int w, int h) { inventoryShape.width = w; inventoryShape.height = h; Changes++; }
    public void Validate()
    {
        foreach (var item in Items)
            if (item.X >= inventoryShape.width || item.Y >= inventoryShape.height) throw new Exception("Item out of bounds");
    }
}
public sealed class PixelWindow { public void Validate() { } }
public sealed class EmporiumEntry
{
    public static EmporiumEntry? Instance;
    public GameGridInventory invElement = new GameGridInventory(24, 10);
    public GameGridInventory backInventory = new GameGridInventory(18, 4);
    public PixelWindow invWindow = new PixelWindow();
    public void SetSmallInv() { }
}
internal static class Program
{
    private static int passed;
    private static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception("FAIL: " + name);
        passed++;
        Console.WriteLine("PASS: " + name);
    }
    private static void Small(EmporiumEntry entry)
    {
        if (SmallInventoryPatch.Prefix(entry)) entry.invElement.SetShape(StartingPerk.IsAscetic() ? 16 : 20, 10);
    }
    private static void Main()
    {
        var mod = new ModEntry();
        mod.Init(new ModManifest());
        mod.OnEnable();
        Check(SmallInventoryPatch.Prefix(new EmporiumEntry()), "menu without store permits vanilla");
        var store = new PlayerStore();
        PlayerStore.Instance = store;
        var entry = new EmporiumEntry();
        EmporiumEntry.Instance = entry;
        store.startingPerks.Add("ascetic");
        Small(entry);
        Check(entry.invElement.inventoryShape.width == 16, "ascetic before purchase remains 16x10");
        StoreOwnedPatch.Postfix(store);
        Check(entry.invElement.inventoryShape.width == 16, "unpaid ownership callback does not unlock (mortgage down payment)");
        entry.invElement.Items.Add((15, 9));
        store.IsPropertyPaid = true;
        StoreOwnedPatch.Postfix(store);
        Check(entry.invElement.inventoryShape.width == 24 && entry.invElement.inventoryShape.height == 10, "fully paid ownership restores 24x10");
        Check(entry.invElement.Items.Count == 1 && entry.invElement.Items[0] == (15, 9), "existing items retain positions");
        Check(store.Cash == 123 && store.startingPerks.Count == 1 && store.startingPerks[0] == "ascetic" && entry.backInventory.Changes == 0, "money, perk selection and other inventory unchanged");
        int changes = entry.invElement.Changes;
        StoreOwnedPatch.Postfix(store);
        Check(entry.invElement.Changes == changes, "repeated ownership callback is idempotent");
        // Simulate a fresh scene and the inspected load order: SetSmallInv, then decode items.
        entry = new EmporiumEntry();
        EmporiumEntry.Instance = entry;
        Small(entry);
        entry.invElement.Items.Add((23, 9));
        entry.invElement.Validate();
        Check(entry.invElement.Changes == 0 && entry.invElement.inventoryShape.width == 24, "paid ascetic load never shrinks before decoding rightmost items");
        entry.invElement.SetShape(16, 10);
        entry.invElement.Items.Clear();
        Small(entry);
        Check(entry.invElement.inventoryShape.width == 24, "preexisting small grid is restored through load entry");
        entry.invElement.SetShape(28, 10);
        StoreOwnedPatch.Postfix(store);
        Check(entry.invElement.inventoryShape.width == 28, "larger existing grid is not reduced");
        store.startingPerks.Clear();
        store.startingPerks.Add("minimalist");
        entry.invElement.SetShape(20, 10);
        StoreOwnedPatch.Postfix(store);
        Check(entry.invElement.inventoryShape.width == 20 && SmallInventoryPatch.Prefix(entry), "minimalist ownership and small-inventory rules stay vanilla");
        store.startingPerks.Clear();
        entry.invElement.SetShape(24, 10);
        StoreOwnedPatch.Postfix(store);
        Check(entry.invElement.inventoryShape.width == 24 && SmallInventoryPatch.Prefix(entry), "no ascetic remains vanilla after purchase");
        store.startingPerks.Add("ascetic");
        entry.invElement.SetShape(16, 10);
        mod.OnDisable();
        Check(SmallInventoryPatch.Prefix(entry), "disabled prefix permits vanilla");
        StoreOwnedPatch.Postfix(store);
        Check(entry.invElement.inventoryShape.width == 16, "disabled ownership patch is inert");
        mod.OnEnable();
        StartingPerk.ThrowOnRead = true;
        Check(SmallInventoryPatch.Prefix(entry), "read failure permits vanilla");
        StoreOwnedPatch.Postfix(store);
        Check(ModLog.Warnings == 1, "failures warn once per enable");
        StartingPerk.ThrowOnRead = false;
        store.IsPropertyPaid = false;
        entry = new EmporiumEntry();
        Small(entry);
        Check(entry.invElement.inventoryShape.width == 16, "switching from paid to unpaid run has no cached unlock");
        Console.WriteLine(passed + " behavioral checks passed.");
    }
}
