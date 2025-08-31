using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;

namespace Cupid;

[BepInAutoPlugin("yanpla.Cupid", "Cupid")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class CupidPlugin : BasePlugin, IMiraPlugin
{
    public Harmony Harmony { get; } = new(Id);

    public string OptionsTitleText => "Cupid";

    public ConfigFile GetConfigFile() => Config;

    public override void Load()
    {
        ReactorCredits.Register(Name, Version, false, ReactorCredits.AlwaysShow);
        Harmony.PatchAll();
    }
}