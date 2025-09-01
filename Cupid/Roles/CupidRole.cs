using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
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
}