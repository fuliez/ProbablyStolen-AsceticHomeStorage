// Development-only inspection harness. Never executes gameplay or writes saves.
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

internal static class GameCheck
{
    private static string managed;
    private static int Main(string[] args)
    {
        managed = args[0];
        AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
        {
            var path = Path.Combine(managed, new AssemblyName(e.Name).Name + ".dll");
            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        };
        try { return Run(args[1]); }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }
    private static void VoidInstance(Type type, string name)
    {
        var method = type.GetMethod(name, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        Require(method != null && method.ReturnType == typeof(void), "Changed target: " + type.Name + "." + name);
    }
    private static int Run(string folder)
    {
        VoidInstance(typeof(EmporiumEntry), "SetSmallInv");
        VoidInstance(typeof(PlayerStore), "OnStoreOwned");
        VoidInstance(typeof(PlayerStore), "BuyOutStore");
        VoidInstance(typeof(PlayerStore), "OnMortgagePaidOff");
        Require(typeof(PlayerStore).GetField("IsPropertyPaid").FieldType == typeof(bool), "Ownership field changed");
        Require(typeof(StartingPerk).GetMethod("IsAscetic").ReturnType == typeof(bool), "Ascetic query changed");
        Require(typeof(EmporiumEntry).GetProperty("invElement")?.PropertyType == typeof(GameGridInventory), "Main grid changed");
        Require(typeof(GameGridInventory).GetProperty("inventoryShape") != null, "Shape API changed");
        Require(typeof(GameGridInventory).GetMethod("SetShape", new[] { typeof(int), typeof(int) }) != null, "Resize API changed");
        Console.WriteLine("PASS: actual installed-game target and storage API signatures.");
        var manifest = XDocument.Load(Path.Combine(folder, "manifest.xml")).Root;
        var normalize = typeof(ModUIManager).GetMethod("NormalizeVersion", BindingFlags.Static | BindingFlags.NonPublic);
        Require(normalize != null, "Version matcher changed");
        var version = (string)normalize.Invoke(null, new object[] { "0.46D" });
        Require(manifest.Element("GameVersions").Elements("Version").Any(v => (string)normalize.Invoke(null, new object[] { v.Value }) == version), "Version mismatch");
        Console.WriteLine("PASS: actual game version normalizer matches 0.46D.");
        var result = ModScanner.Scan(System.IO.Directory.GetFiles(folder, "*.dll"), System.IO.Directory.GetFiles(managed, "*.dll").Select(Path.GetFileNameWithoutExtension), folder, true);
        Console.WriteLine("Scanner: methods=" + result.MethodsScanned + ", refused=" + result.Refused + ", capabilities=" + result.Capabilities.Count + ", hardFlags=" + result.HardFlags.Count + ", error=" + (result.Error ?? "none"));
        foreach (var flag in result.HardFlags) Console.WriteLine("HARD: " + flag);
        foreach (var finding in result.Findings) Console.WriteLine("FIND: " + finding);
        return result.Refused || result.Capabilities.Count != 0 || result.HardFlags.Count != 0 || result.Error != null ? 1 : 0;
    }
}
