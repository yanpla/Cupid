using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace Cupid.Roles;

public class CupidRole : CrewmateRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public static List<PlayerControl> SelectedPlayers { get; } = new();
    
    public string RoleName => "Cupid";
    public string RoleLongDescription => "Spread love between two players.\n" +
                                         "If one lover dies, the other dies too.\n";
    public string RoleDescription => "Spread love between two players.";
    
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    
    public Color RoleColor { get; } = Color.magenta;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleOptionsGroup RoleOptionsGroup { get; } = RoleOptionsGroup.Crewmate;
    public DoomableType DoomHintType { get; } = DoomableType.Trickster;
    public CustomRoleConfiguration Configuration => new()
    {
        MaxRoleCount = 1,
        OptionsScreenshot = MiraAssets.Empty,
        Icon = Assets.CupidShootButton,
        CanGetKilled = true,
        UseVanillaKillButton = false,
        CanUseVent = false,
        TasksCountForProgress = true,
        CanUseSabotage = false,
        DefaultChance = 100,
        DefaultRoleCount = 1,
        CanModifyChance = true,
        RoleHintType = RoleHintType.RoleTab,
        ShowInFreeplay = true,
    };
    
    public string GetAdvancedDescription()
    {
        return
            $"The {RoleName} is a Crewmate Power role that can shoot arrows to bind two players together as Lovers. If one Lover dies, the other will follow. (Make sure to shoot before the first meeting!)" +
            MiscUtils.AppendOptionsText(GetType());
    }
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities { get; } =
    [
        new("Shoot love arrows",
            "Fire a magical arrow at a player to mark them as a potential Lover." + 
            " When two players are marked, they will be bound together as Lovers at the end of the first meeting.",
            Assets.CupidShootButton)
    ];

    public static void onRoundStart()
    {
        if (SelectedPlayers.Count != 2 || SelectedPlayers.Any(p => p.Data == null || p.Data.Disconnected || p.Data.IsDead))
        { 
            foreach (var p in SelectedPlayers)
                p.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(Color.magenta));

            SelectedPlayers.Clear();
            PlayerControl.LocalPlayer.RpcChangeRole(0); // Revert to Crewmate
            return;
        }

        RpcSetOtherLover(
            SelectedPlayers[0], 
            SelectedPlayers[1]
        );
    }
    
    [MethodRpc((uint)TownOfUsRpc.SetOtherLover)]
    private static void RpcSetOtherLover(PlayerControl player, PlayerControl target)
    {
        if (PlayerControl.AllPlayerControls.ToArray().Where(x => x.HasModifier<LoverModifier>()).ToList().Count > 0)
        {
            Logger<CupidPlugin>.Error("RpcSetOtherLover - Lovers Already Spawned!");
            return;
        }

        var targetModifier = target.AddModifier<LoverModifier>();
        var sourceModifier = player.AddModifier<LoverModifier>();
        targetModifier!.OtherLover = player;
        sourceModifier!.OtherLover = target;
    }
}