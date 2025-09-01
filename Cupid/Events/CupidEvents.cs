
using AmongUs.GameOptions;
using Cupid.Buttons;
using Cupid.Roles;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using UnityEngine;

namespace Cupid.Events;

public static class CupidEvents
{
  [RegisterEvent]
  public static void HandleEndMeetingEvent(EndMeetingEvent @event)
  {
    if (PlayerControl.LocalPlayer.Data.Role is not CupidRole) return;

    // Snapshot current selection (ignore nulls)
    var selected = CupidRole.SelectedPlayers
      .Where(p => p != null)
      .ToList();

    // Need exactly two.
    if (selected.Count != 2)
    {
      CleanupAndRevert();
      return;
    }

    if (selected.Any(NotAlive))
    {
      CleanupAndRevert();
      return;
    }

    // If a selected player was exiled this meeting, cancel.
    // exiledPlayer may be null or use 255 when nobody is exiled; handle both.
    var exiledId = byte.MaxValue;
    var exiled = @event.MeetingHud.exiledPlayer;
    if (exiled != null) exiledId = exiled.PlayerId;

    if (selected.Any(p => p.PlayerId == exiledId))
    {
      CleanupAndRevert();
      return;
    }

    // Find the Lovers modifier (by name) and apply to both.
    var lover = ModifierManager.Modifiers.FirstOrDefault(
      m => string.Equals(m.ModifierName, "Lover", StringComparison.OrdinalIgnoreCase)
    );

    if (lover == null)
    {
      Logger<CupidPlugin>.Error($"No lover modifier found! Cannot apply.");
      CleanupAndRevert();
      return;
    }

    selected[0].RpcAddModifier(lover.TypeId);
    selected[1].RpcAddModifier(lover.TypeId);

    return;

    // Helper cleanup: clear outlines, clear selection, revert Cupid.
    void CleanupAndRevert()
    {
      foreach (var p in selected)
      {
        p.cosmetics.SetOutline(
          false,
          new Il2CppSystem.Nullable<Color>(Color.magenta)
        );
      }

      CupidRole.SelectedPlayers.Clear();
      PlayerControl.LocalPlayer.RpcSetRole(RoleTypes.Crewmate);
    }

    // If anyone is dead/disconnected at vote completion, cancel.
    bool NotAlive(PlayerControl p) =>
      p.Data == null || p.Data.Disconnected || p.Data.IsDead;
  }
  
  public static void HandlePlayerDeathEvent(PlayerDeathEvent @event)
  {
    if (PlayerControl.LocalPlayer.Data.Role is not CupidRole) return;

    var player = @event.Player;
    if (player == PlayerControl.LocalPlayer)
    {
      // If Cupid dies, clear selection.
      foreach (var p in CupidRole.SelectedPlayers)
      {
        p.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(Color.magenta));
      }
    
      CupidRole.SelectedPlayers.Clear();
      return;
    }
    
    // Snapshot current selection (ignore nulls)
    var selected = CupidRole.SelectedPlayers
      .Where(p => p != null)
      .ToList();
    
    if (!selected.Contains(player)) return;
    
    // If selected player dies.
    player.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(Color.magenta));
    selected.Remove(player);
    var cupidButton = CustomButtonSingleton<CupidShootButton>.Instance;
    cupidButton.IncreaseUses();
    cupidButton.DecreaseTimer(5f);
  }
}