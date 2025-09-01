using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using TownOfUs.Utilities;
using UnityEngine;

namespace Cupid.Roles;

public class CupidRole : CrewmateRole, ICustomRole
{
    public static List<PlayerControl> SelectedPlayers { get; } = new();
    
    public string RoleName => "Cupid";
    public string RoleLongDescription => "Make two players fall in love. If one lover dies, the other dies too.";
    public string RoleDescription => RoleLongDescription;
    public Color RoleColor { get; } = Color.magenta;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleOptionsGroup RoleOptionsGroup { get; } = RoleOptionsGroup.Crewmate;
    public CustomRoleConfiguration Configuration => new()
    {
        MaxRoleCount = 1,
        OptionsScreenshot = MiraAssets.Empty,
        Icon = MiraAssets.Empty,
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

        TownOfUs.Modifiers.Game.Alliance.LoverModifier.RpcSetOtherLover(
            SelectedPlayers[0], 
            SelectedPlayers[1]
        );
    }
}